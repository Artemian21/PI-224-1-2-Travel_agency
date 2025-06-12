import React, { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from 'react-query'
import { hotelAPI } from '../../services/api'

export const useManageHotels = () => {
  const queryClient = useQueryClient()

  const [deleteHotelId, setDeleteHotelId] = useState(null)
  const [editingHotel, setEditingHotel] = useState(null)
  const [form, setForm] = useState({
    name: '',
    country: '',
    city: '',
    address: '',
  })

  const { data: hotels, isLoading, error } = useQuery('hotels', hotelAPI.getAll, {
    select: response => response.data,
  })

  const [searchTerm, setSearchTerm] = useState('')

  const filteredHotels = React.useMemo(() => {
    if (!hotels) return []
    if (!searchTerm) return hotels
    return hotels.filter(hotel =>
      hotel.name.toLowerCase().includes(searchTerm.toLowerCase())
    )
  }, [hotels, searchTerm])

  const createMutation = useMutation(hotelAPI.create, {
    onSuccess: () => {
      queryClient.invalidateQueries('hotels')
      setEditingHotel(null)
      resetForm()
    },
  })

  const updateMutation = useMutation(({ id, updatedHotel }) => hotelAPI.update(id, updatedHotel), {
    onSuccess: () => {
      queryClient.invalidateQueries('hotels')
      setEditingHotel(null)
      resetForm()
    },
  })

  const deleteMutation = useMutation(hotelAPI.delete, {
    onSuccess: () => queryClient.invalidateQueries('hotels'),
  })

  const resetForm = () => {
    setForm({
      name: '',
      country: '',
      city: '',
      address: '',
    })
  }

  const onChangeForm = (e) => {
    const { name, value } = e.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const onSubmit = (e) => {
    e.preventDefault()
    if (!form.name || !form.country || !form.city || !form.address) {
      alert('Заповніть всі поля')
      return
    }
    if (editingHotel === 'new') {
      createMutation.mutate(form)
    } else if (editingHotel && editingHotel.id) {
      updateMutation.mutate({ id: editingHotel.id, updatedHotel: form })
    }
  }

  const onEdit = (hotel) => {
    setEditingHotel(hotel)
    setForm({
      name: hotel.name || '',
      country: hotel.country || '',
      city: hotel.city || '',
      address: hotel.address || '',
    })
  }

  const handleDelete = (id) => {
    setDeleteHotelId(id)
  }

  const confirmDelete = () => {
    if (deleteHotelId) {
      deleteMutation.mutate(deleteHotelId)
      setDeleteHotelId(null)
    }
  }

  const cancelDelete = () => {
    setDeleteHotelId(null)
  }

  return {
    hotels,
    isLoading,
    error,
    editingHotel,
    filteredHotels,
    setSearchTerm,
    setEditingHotel,
    form,
    onChangeForm,
    onSubmit,
    onEdit,
    handleDelete,
    deleteHotelId,
    confirmDelete,
    cancelDelete,
    resetForm,
    createOrUpdateLoading: createMutation.isLoading || updateMutation.isLoading,
  }
}
