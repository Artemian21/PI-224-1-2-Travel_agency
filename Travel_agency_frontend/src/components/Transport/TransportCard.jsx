import React, { useState } from 'react'
import { useMutation } from 'react-query'
import { useAuth } from '../../contexts/AuthContext'
import { useNavigate } from 'react-router-dom'
import { ticketBookingAPI } from '../../services/api'
import { formatDateTime, formatPrice } from '../../utils/formatters'
import { Plane, Calendar, Building } from 'lucide-react'

const TransportCard = ({ transport }) => {
  const { isAuthenticated } = useAuth()
  const navigate = useNavigate()
  const [isBooking, setIsBooking] = useState(false)

  const bookingMutation = useMutation(
    (transportId) =>
      ticketBookingAPI.create({ transportId, status: 'Pending' }),
    {
      onSuccess: () => {
        alert('Ticket successfully booked!')
        navigate('/profile')
      },
      onError: () => {
        alert('Booking failed.')
        setIsBooking(false)
      }
    }
  )

  const handleBooking = () => {
    if (!isAuthenticated) {
      navigate('/login')
      return
    }
    setIsBooking(true)
    bookingMutation.mutate(transport.id)
  }

  return (
    <div
      className="rounded-2xl shadow-sm border hover:shadow-md transition-all cursor-pointer bg-white p-5 space-y-4"
      onClick={() => navigate(`/transports/${transport.id}`)}
    >
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-2 text-blue-600">
          <Plane className="h-5 w-5" />
          <span className="font-medium text-gray-800">{transport.type}</span>
        </div>
        <span className="text-xl font-bold text-green-600">
          {formatPrice(transport.price)}
        </span>
      </div>

      <div className="flex items-center gap-2 text-sm text-gray-500">
        <Building className="h-4 w-4" />
        <span>{transport.company}</span>
      </div>

      <div className="space-y-1 text-sm text-gray-600">
        <div className="flex items-center gap-2">
          <Calendar className="h-4 w-4" />
          <span>Departure: {formatDateTime(transport.departureDate)}</span>
        </div>
        <div className="flex items-center gap-2">
          <Calendar className="h-4 w-4" />
          <span>Arrival: {formatDateTime(transport.arrivalDate)}</span>
        </div>
      </div>

      <button
        onClick={(e) => {
          e.stopPropagation()
          handleBooking()
        }}
        disabled={isBooking || bookingMutation.isLoading}
        className="w-full rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 transition disabled:opacity-50"
      >
        {isBooking || bookingMutation.isLoading
          ? 'Booking...'
          : 'Book Ticket'}
      </button>
    </div>
  )
}

export default TransportCard