import { useState, useEffect, useMemo } from 'react';
import { useQuery, useMutation, useQueryClient } from 'react-query';
import { hotelAPI, hotelRoomAPI } from '../../services/api';
import LoadingSpinner from '../UI/LoadingSpinner';

export const useManageRooms = () => {
  const queryClient = useQueryClient();

  const [form, setForm] = useState({
    id: null,
    hotelId: '',
    roomType: 'Single',
    capacity: 0,
    pricePerNight: 0,
  });
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [deleteRoomId, setDeleteRoomId] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  const { data: hotels, isLoading: hotelsLoading, error: hotelsError } = useQuery('hotels', hotelAPI.getAll, {
    select: response => response.data,
  })

  const {
    data: rooms = [],
    isLoading: roomsLoading,
    error: roomsError,
  } = useQuery('rooms', () => hotelRoomAPI.getAll().then(res => res.data));

  const filteredRooms = useMemo(() => {
    if (!rooms) return [];
    return rooms.filter(room =>
      room.roomType?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      room.hotel?.name?.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [rooms, searchTerm]);

  const roomsWithHotelInfo = rooms.map(room => {
    const hotel = Array.isArray(hotels) ? hotels.find(h => h.id === room.hotelId) : null;
    return {
      ...room,
      hotelName: hotel?.name || 'Невідомий готель',
      hotelAddress: hotel ? `${hotel.country}, ${hotel.city}, ${hotel.address}` : '',
    };
  });

  const saveRoomMutation = useMutation(
    (payload) =>
      payload.id
        ? hotelRoomAPI.update(payload.id, payload)
        : hotelRoomAPI.create(payload),
    {
      onSuccess: () => {
        queryClient.invalidateQueries('rooms');
        setForm({
          id: null,
          hotelId: '',
          roomType: 'Single',
          capacity: 0,
          pricePerNight: 0,
        });
        setIsEditing(false); // Додаємо це для закриття форми
        setError(null);
      },
      onError: () => setError('Помилка збереження кімнати'),
    }
  );

  const deleteRoomMutation = useMutation(
    (id) => hotelRoomAPI.delete(id),
    {
      onSuccess: () => {
        queryClient.invalidateQueries('rooms');
        setError(null);
      },
      onError: () => setError('Помилка видалення кімнати'),
    }
  );

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: name === 'capacity' || name === 'pricePerNight' ? Number(value) : value,
    }));
  };

  const handleEdit = (room) => {
    setForm({
      id: room.id,
      hotelId: room.hotelId,
      roomType: room.roomType,
      capacity: room.capacity,
      pricePerNight: room.pricePerNight,
    });
    setIsEditing(true);
    setError(null);
  };

  const handleCancel = () => {
    setForm({
      id: null,
      hotelId: '',
      roomType: 'Single',
      capacity: 0,
      pricePerNight: 0,
    });
    setIsEditing(false);
    setError(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!form.hotelId) return setError('Обов\'язково обрати готель');
    if (form.capacity < 0) return setError('Кількість місць не може бути від\'ємною');
    try {
      await saveRoomMutation.mutateAsync(form);
    } catch { }
  };

  const confirmDelete = () => {
    if (deleteRoomId) {
      deleteRoomMutation.mutate(deleteRoomId);
      setDeleteRoomId(null);
    }
  };

  return {
    hotels,
    hotelsLoading,
    hotelsError,
    rooms: roomsWithHotelInfo,
    roomsLoading,
    roomsError,
    form,
    error,
    filteredRooms: roomsWithHotelInfo,
    searchTerm,
    deleteRoomId,
    isEditing,
    setSearchTerm,
    setDeleteRoomId,
    onEdit: handleEdit,
    onCancel: handleCancel,
    confirmDelete,
    cancelDelete: () => setDeleteRoomId(null),
    handleChange,
    handleSubmit,
    saveRoomLoading: saveRoomMutation.isLoading,
    deleteRoomLoading: deleteRoomMutation.isLoading,
  };
};