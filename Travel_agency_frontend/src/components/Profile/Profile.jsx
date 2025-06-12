import React, { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from 'react-query'
import { tourBookingAPI, hotelBookingAPI, ticketBookingAPI, hotelAPI } from '../../services/api'
import { useAuth } from '../../contexts/AuthContext'
import LoadingSpinner from '../UI/LoadingSpinner'
import BookingCard from './BookingCard'
import { User, Calendar, Edit } from 'lucide-react'
import { Link } from 'react-router-dom'

const Profile = () => {
  const { user, canViewOwnBookings = true, canEditOwnProfile = true } = useAuth()
  const queryClient = useQueryClient()
  const [activeTab, setActiveTab] = useState('bookings')
  const [bookingToCancel, setBookingToCancel] = useState(null)

  const fetchDetailedBookings = async (api, label) => {
    const res = await api.getAll()
    const userBookings = res.data.filter(b => b.userId === user?.id)
    return Promise.all(
      userBookings.map(b =>
        api.getById(b.id).then(d => ({ ...d.data, type: label }))
      )
    )
  }

  const canFetchBookings = Boolean(user) && Boolean(canViewOwnBookings)

  const { data: tourBookings = [], isLoading: tourLoading } = useQuery(
    ['user-tour-bookings', user?.id],
    () => fetchDetailedBookings(tourBookingAPI, 'tour'),
    { enabled: canFetchBookings }
  )

  const { data: hotelBookings = [], isLoading: hotelLoading } = useQuery(
    ['user-hotel-bookings', user?.id],
    async () => {
      const bookings = await fetchDetailedBookings(hotelBookingAPI, 'hotel')
      const bookingsWithHotelName = await Promise.all(
        bookings.map(async booking => {
          if (!booking.hotelRoom?.hotelId) return booking
          const hotel = await hotelAPI.getById(booking.hotelRoom.hotelId)
          return {
            ...booking,
            hotelName: hotel?.data?.name || 'Unnamed Hotel'
          }
        })
      )
      return bookingsWithHotelName
    },
    { enabled: canFetchBookings }
  )

  const { data: ticketBookings = [], isLoading: ticketLoading } = useQuery(
    ['user-ticket-bookings', user?.id],
    () => fetchDetailedBookings(ticketBookingAPI, 'ticket'),
    { enabled: canFetchBookings }
  )

  const cancelBookingMutation = useMutation(
    async ({ type, booking }) => {
      const apis = { tour: tourBookingAPI, hotel: hotelBookingAPI, ticket: ticketBookingAPI }
      if (!apis[type]) throw new Error(`Unknown booking type: ${type}`)
      return apis[type].update(booking.id, { ...booking, status: 'Cancelled' })
    },
    {
      onSuccess: (_, { type }) => {
        queryClient.invalidateQueries([`user-${type}-bookings`, user?.id])
      }
    }
  )

  const handleCancelBooking = booking => setBookingToCancel(booking)

  const confirmCancel = () => {
    if (bookingToCancel) {
      cancelBookingMutation.mutate({ type: bookingToCancel.type, booking: bookingToCancel })
      setBookingToCancel(null)
    }
  }

  const closeModal = () => setBookingToCancel(null)

  const isLoading = tourLoading || hotelLoading || ticketLoading

  const allBookings = [...tourBookings, ...hotelBookings, ...ticketBookings].sort(
    (a, b) => new Date(b.bookingDate) - new Date(a.bookingDate)
  )

  if (isLoading) return <LoadingSpinner />

  return (
    <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8 space-y-8">
      <section className="bg-white shadow rounded-lg p-6 flex items-center justify-between">
        <div className="flex items-center space-x-5">
          <div className="h-20 w-20 rounded-full bg-blue-600 flex items-center justify-center shadow-md">
            <User className="h-10 w-10 text-white" />
          </div>
          <div>
            <h1 className="text-3xl font-semibold text-gray-900">{user?.username}</h1>
            <p className="text-gray-500 mt-1">{user?.email}</p>
            <span className="inline-block mt-2 px-3 py-1 text-sm font-semibold bg-blue-100 text-blue-800 rounded-full shadow">
              {user?.role}
            </span>
          </div>
        </div>
        {canEditOwnProfile && (
          <Link
            to="/profile/edit"
            className="inline-flex items-center space-x-2 text-blue-600 hover:text-blue-800 font-medium focus:outline-none focus:ring-2 focus:ring-blue-500 rounded"
          >
            <Edit className="h-5 w-5" />
            <span>Edit Profile</span>
          </Link>
        )}
      </section>

      <nav className="border-b border-gray-200">
        <ul className="flex space-x-8" role="tablist">
          {canViewOwnBookings && (
            <li>
              <button
                role="tab"
                aria-selected={activeTab === 'bookings'}
                onClick={() => setActiveTab('bookings')}
                className={`py-3 text-sm font-semibold transition-colors duration-200 ${
                  activeTab === 'bookings'
                    ? 'border-b-4 border-blue-600 text-blue-600'
                    : 'border-b-4 border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
                }`}
              >
                My Bookings ({allBookings.length})
              </button>
            </li>
          )}
          <li>
            <button
              role="tab"
              aria-selected={activeTab === 'settings'}
              onClick={() => setActiveTab('settings')}
              className={`py-3 text-sm font-semibold transition-colors duration-200 ${
                activeTab === 'settings'
                  ? 'border-b-4 border-blue-600 text-blue-600'
                  : 'border-b-4 border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
              }`}
            >
              Account Settings
            </button>
          </li>
        </ul>
      </nav>

      {activeTab === 'bookings' && canViewOwnBookings && (
        <section>
          <h2 className="text-2xl font-semibold mb-6 text-gray-900">My Bookings</h2>
          {allBookings.length === 0 ? (
            <div className="bg-white rounded-lg shadow-md p-10 text-center">
              <Calendar className="mx-auto mb-4 h-14 w-14 text-gray-300" />
              <h3 className="text-xl font-medium text-gray-900 mb-2">No bookings yet</h3>
              <p className="text-gray-500 mb-6 max-w-md mx-auto">
                Start exploring our tours, hotels, and transport options!
              </p>
              <div className="flex justify-center space-x-6">
                <Link
                  to="/tours"
                  className="btn-primary px-6 py-2 rounded shadow hover:bg-blue-700 transition"
                >
                  Browse Tours
                </Link>
                <Link
                  to="/hotels"
                  className="btn-secondary px-6 py-2 rounded shadow hover:bg-gray-200 transition"
                >
                  Find Hotels
                </Link>
              </div>
            </div>
          ) : (
            <div className="grid gap-6 md:grid-cols-2">
              {allBookings.map(booking => (
                <BookingCard
                  key={`${booking.type}-${booking.id}`}
                  booking={booking}
                  canCancel={booking.status !== 'Cancelled' && booking.status !== 'Completed'}
                  showActions
                  onCancel={handleCancelBooking}
                />
              ))}
            </div>
          )}
        </section>
      )}

      {activeTab === 'settings' && (
        <section>
          <h2 className="text-2xl font-semibold mb-6 text-gray-900">Account Settings</h2>
          <div className="bg-white rounded-lg shadow-md p-6 space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-700">Username</label>
              <p className="mt-1 text-gray-900 text-lg">{user?.username}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Email</label>
              <p className="mt-1 text-gray-900 text-lg">{user?.email}</p>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">Role</label>
              <p className="mt-1 text-gray-900 text-lg">{user?.role}</p>
            </div>
          </div>
        </section>
      )}

      {bookingToCancel && (
        <div
          className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50 px-4"
          role="dialog"
          aria-modal="true"
          aria-labelledby="cancel-dialog-title"
        >
          <div className="bg-white rounded-lg shadow-xl max-w-md w-full p-6 space-y-5">
            <h2 id="cancel-dialog-title" className="text-xl font-semibold text-gray-900">
              Cancel Booking
            </h2>
            <p className="text-gray-700">
              Are you sure you want to cancel your{' '}
              <span className="font-medium capitalize">{bookingToCancel.type}</span> booking (ID:{' '}
              <span className="font-mono">{bookingToCancel.id}</span>)?
            </p>
            <div className="flex justify-end space-x-4">
              <button
                onClick={closeModal}
                className="px-5 py-2 rounded-md bg-gray-200 text-gray-800 hover:bg-gray-300 transition focus:outline-none focus:ring-2 focus:ring-gray-400"
              >
                No
              </button>
              <button
                onClick={confirmCancel}
                className="px-5 py-2 rounded-md bg-red-600 text-white hover:bg-red-700 transition focus:outline-none focus:ring-2 focus:ring-red-500"
              >
                Yes, Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default Profile