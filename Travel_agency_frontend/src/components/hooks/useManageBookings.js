import React, { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from 'react-query'
import { tourBookingAPI, hotelBookingAPI, ticketBookingAPI, userAPI } from '../../services/api'
import { useAuth } from '../../contexts/AuthContext'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import { Calendar, Filter, User, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'

export const useManageBookings = () => {
  const queryClient = useQueryClient()

  const [searchTerm, setSearchTerm] = useState('')
  const [statusFilter, setStatusFilter] = useState('All')
  const [typeFilter, setTypeFilter] = useState('All')
  const [selectedBooking, setSelectedBooking] = useState(null)
  const [selectedUser, setSelectedUser] = useState(null)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [deleteTarget, setDeleteTarget] = useState(null)

  // Запити бронювань
  const { data: tourBookings } = useQuery(['tour-bookings'], () => tourBookingAPI.getAll())
  const { data: hotelBookings } = useQuery(['hotel-bookings'], () => hotelBookingAPI.getAll())
  const { data: ticketBookings } = useQuery(['ticket-bookings'], () => ticketBookingAPI.getAll())

  // Збір userIds
  const userIds = [
    ...new Set([
      ...(tourBookings?.data?.map(b => b.userId) || []),
      ...(hotelBookings?.data?.map(b => b.userId) || []),
      ...(ticketBookings?.data?.map(b => b.userId) || []),
    ]),
  ]

  // Запит користувачів
  const { data: usersData, isLoading: isUsersLoading } = useQuery(
    ['users', userIds],
    async () => {
      const users = await Promise.all(userIds.map(id => userAPI.getById(id)))
      return users.map(res => res.data)
    },
    {
      enabled: userIds.length > 0,
      staleTime: 5 * 60 * 1000,
    }
  )

  // Мутації
  const updateBookingMutation = useMutation(
    ({ type, id, updatedBooking }) => {
      const apis = {
        tour: tourBookingAPI,
        hotel: hotelBookingAPI,
        ticket: ticketBookingAPI,
      }
      return apis[type].update(id, updatedBooking)
    },
    {
      onSuccess: (data, variables) => {
        queryClient.invalidateQueries([`${variables.type}-bookings`])
      },
    }
  )

  const deleteBookingMutation = useMutation(
    ({ type, id }) => {
      const apis = {
        tour: tourBookingAPI,
        hotel: hotelBookingAPI,
        ticket: ticketBookingAPI,
      }
      return apis[type].delete(id)
    },
    {
      onSuccess: (data, variables) => {
        queryClient.invalidateQueries([`${variables.type}-bookings`])
      },
    }
  )

  // Хендлери
  const handleStatusChange = (type, id, newStatus) => {
    const allBookings = [
      ...(tourBookings?.data?.map(b => ({ ...b, type: 'tour' })) || []),
      ...(hotelBookings?.data?.map(b => ({ ...b, type: 'hotel' })) || []),
      ...(ticketBookings?.data?.map(b => ({ ...b, type: 'ticket' })) || []),
    ]
    const bookingToUpdate = allBookings.find(b => b.id === id && b.type === type)
    if (!bookingToUpdate) return
  
    const updatedBooking = { ...bookingToUpdate, status: newStatus }
    updateBookingMutation.mutate({ type, id, updatedBooking })
  }

  const handleDeleteBooking = (type, id) => {
    setDeleteTarget({ type, id })
    setIsDeleteModalOpen(true)
  }

  const confirmDelete = () => {
    if (deleteTarget) {
      deleteBookingMutation.mutate(deleteTarget)
      setIsDeleteModalOpen(false)
      setDeleteTarget(null)
    }
  }

  const cancelDelete = () => {
    setIsDeleteModalOpen(false)
    setDeleteTarget(null)
  }

  // Всі бронювання з типом
  const allBookings = [
    ...(tourBookings?.data?.map((b) => ({ ...b, type: 'tour' })) || []),
    ...(hotelBookings?.data?.map((b) => ({ ...b, type: 'hotel' })) || []),
    ...(ticketBookings?.data?.map((b) => ({ ...b, type: 'ticket' })) || []),
  ]

  // Фільтрація
  const filteredBookings = allBookings.filter((booking) => {
    const user = usersData?.find(u => u.id === booking.userId)
    const email = user?.email || ''
    const matchesSearch =
      email.toLowerCase().includes(searchTerm.toLowerCase()) ||
      booking.type.toLowerCase().includes(searchTerm.toLowerCase())

    const matchesStatus = statusFilter === 'All' || booking.status === statusFilter
    const matchesType = typeFilter === 'All' || booking.type === typeFilter.toLowerCase()

    return matchesSearch && matchesStatus && matchesType
  })

  const openDetails = (booking) => {
    setSelectedBooking(booking)
    const user = usersData.find(u => u.id === booking.userId)
    setSelectedUser(user)
  }

  return {
    usersData,
    searchTerm,
    setSearchTerm,
    statusFilter,
    setStatusFilter,
    typeFilter,
    setTypeFilter,
    selectedBooking,
    selectedUser,
    setSelectedBooking,
    setSelectedUser,
    filteredBookings,
    isLoading: !tourBookings || !hotelBookings || !ticketBookings || isUsersLoading,
    handleStatusChange,
    openDetails,
    handleDeleteBooking,
    isDeleteModalOpen,
    confirmDelete,
    cancelDelete,
  }
}