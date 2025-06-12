import React from 'react'
import { MapPin, Building, Plane, Calendar, DollarSign } from 'lucide-react'

const BookingCard = ({ booking, onCancel, canCancel, showActions = false }) => {
  const iconByType = {
    tour: <MapPin className="h-5 w-5" />,
    hotel: <Building className="h-5 w-5" />,
    ticket: <Plane className="h-5 w-5" />
  }

  const statusColors = {
    Pending: 'bg-yellow-100 text-yellow-800',
    Confirmed: 'bg-green-100 text-green-800',
    Cancelled: 'bg-red-100 text-red-800',
    Rejected: 'bg-red-200 text-red-900',
    Paid: 'bg-indigo-100 text-indigo-800',
    Completed: 'bg-blue-100 text-blue-800'
  }

  const typeLabels = {
    tour: 'Tour Booking',
    hotel: 'Hotel Booking',
    ticket: 'Transport Booking'
  }

  const formatDate = (dateString) =>
    new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    })

  const titleByType = () => {
    if (booking.type === 'tour') return booking.tour?.name || 'Unnamed Tour'
    if (booking.type === 'hotel') return booking.hotelName || 'Unnamed Hotel'
    if (booking.type === 'ticket') return booking.transport?.name || booking.transport?.type || 'Unnamed Transport'
    return 'Booking'
  }

  return (
    <div className="card p-4 border rounded-md hover:shadow-lg transition-shadow">
      <div className="flex justify-between items-start mb-4">
        <div className="flex items-center space-x-2">
          <div className="p-2 rounded-lg bg-blue-100">{iconByType[booking.type] || null}</div>
          <div>
            <h3 className="font-semibold text-gray-900">{typeLabels[booking.type] || 'Booking'}</h3>
            <p className="text-sm text-gray-600">{titleByType()}</p>
          </div>
        </div>
        <span className={`px-2 py-1 rounded-full text-xs font-semibold ${statusColors[booking.status] || 'bg-gray-100 text-gray-800'}`}>
          {booking.status}
        </span>
      </div>

      <div className="mb-4 space-y-2 text-gray-600 text-sm">
        <div className="flex items-center space-x-2">
          <Calendar className="h-4 w-4" />
          <span>Booked: {formatDate(booking.bookingDate || booking.startDate)}</span>
        </div>

        {booking.totalAmount && (
          <div className="flex items-center space-x-2">
            <DollarSign className="h-4 w-4" />
            <span>${booking.totalAmount}</span>
          </div>
        )}

        {booking.numberOfGuests && (
          <div>
            Guests: {booking.numberOfGuests}
          </div>
        )}
      </div>

      {showActions && canCancel && (
        <div>
          <button
            onClick={() => onCancel(booking)}
            className="px-3 py-1 text-sm font-semibold rounded bg-red-600 text-white hover:bg-red-700 transition"
          >
            Cancel
          </button>
        </div>
      )}
    </div>
  )
}

export default BookingCard