import React, { useState } from 'react'
import { useMutation, useQueryClient } from 'react-query'
import { useAuth } from '../../contexts/AuthContext'
import { useNavigate } from 'react-router-dom'
import { hotelBookingAPI } from '../../services/api'
import { formatPrice } from '../../utils/formatters'
import { Users, Bed } from 'lucide-react'
import { useForm } from 'react-hook-form'

const RoomCard = ({ room, hotelName }) => {
  const { isAuthenticated } = useAuth()
  const navigate = useNavigate()
  const [showBookingForm, setShowBookingForm] = useState(false)
  const queryClient = useQueryClient()

  const {
    register,
    handleSubmit,
    formState: { errors }
  } = useForm()

  const bookingMutation = useMutation(
    (bookingData) => hotelBookingAPI.create(bookingData),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(['user-tour-bookings'])
        setShowBookingForm(false)
        navigate('/profile')
      },
      onError: () => {
        alert('Booking error occurred')
      }
    }
  )

  const onSubmit = (data) => {
    const bookingData = {
      hotelRoomId: room.id,
      startDate: data.startDate,
      endDate: data.endDate,
      numberOfGuests: parseInt(data.numberOfGuests, 10),
      status: 'Pending'
    }
    bookingMutation.mutate(bookingData)
  }

  const handleBookingClick = (e) => {
    e.stopPropagation()
    if (!isAuthenticated) {
      navigate('/login')
      return
    }
    setShowBookingForm(true)
  }

  const handleCardClick = () => {
    if (!showBookingForm) {
      navigate(`/rooms/${room.id}`)
    }
  }

  return (
    <div
      className="card cursor-pointer p-6 border rounded-lg shadow-sm hover:shadow-md transition-shadow duration-300"
      onClick={handleCardClick}
      role="button"
      tabIndex={0}
      onKeyPress={(e) => {
        if (e.key === 'Enter') handleCardClick()
      }}
    >
      <div className="space-y-5">
        <div className="flex items-center justify-between">
          <h3 className="text-xl font-semibold text-gray-900">{room.roomType}</h3>
          <span className="text-2xl font-bold text-green-600">
            {formatPrice(room.pricePerNight)}/night
          </span>
        </div>

        <div className="flex items-center space-x-6 text-gray-600 text-sm">
          <div className="flex items-center space-x-1">
            <Users className="h-5 w-5" />
            <span>Up to {room.capacity} guests</span>
          </div>
          <div className="flex items-center space-x-1">
            <Bed className="h-5 w-5" />
            <span>{room.roomType}</span>
          </div>
        </div>

        {!showBookingForm ? (
          <button
            onClick={handleBookingClick}
            className="w-full py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors disabled:opacity-50"
          >
            Book Now
          </button>
        ) : (
          <div onClick={(e) => e.stopPropagation()}>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
              <div>
                <label className="block mb-1 font-medium text-gray-700">Check-in Date</label>
                <input
                  type="date"
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  {...register('startDate', { required: 'This field is required' })}
                />
                {errors.startDate && (
                  <p className="mt-1 text-sm text-red-600">{errors.startDate.message}</p>
                )}
              </div>

              <div>
                <label className="block mb-1 font-medium text-gray-700">Check-out Date</label>
                <input
                  type="date"
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  {...register('endDate', { required: 'This field is required' })}
                />
                {errors.endDate && (
                  <p className="mt-1 text-sm text-red-600">{errors.endDate.message}</p>
                )}
              </div>

              <div>
                <label className="block mb-1 font-medium text-gray-700">Number of Guests</label>
                <input
                  type="number"
                  min="1"
                  max={room.capacity}
                  className="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                  {...register('numberOfGuests', {
                    required: 'This field is required',
                    min: { value: 1, message: 'Minimum 1 guest' },
                    max: { value: room.capacity, message: `Maximum ${room.capacity} guests` }
                  })}
                />
                {errors.numberOfGuests && (
                  <p className="mt-1 text-sm text-red-600">{errors.numberOfGuests.message}</p>
                )}
              </div>

              <div className="flex space-x-3">
                <button
                  type="submit"
                  disabled={bookingMutation.isLoading}
                  className="flex-1 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 transition-colors disabled:opacity-50"
                >
                  {bookingMutation.isLoading ? 'Booking...' : 'Confirm'}
                </button>
                <button
                  type="button"
                  onClick={(e) => {
                    e.stopPropagation()
                    setShowBookingForm(false)
                  }}
                  className="flex-1 py-2 bg-gray-300 text-gray-700 rounded-md hover:bg-gray-400 transition-colors"
                >
                  Cancel
                </button>
              </div>
            </form>
          </div>
        )}
      </div>
    </div>
  )
}

export default RoomCard