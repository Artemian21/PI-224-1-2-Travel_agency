import React from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useQuery, useMutation, useQueryClient } from 'react-query'
import { tourAPI, tourBookingAPI } from '../../services/api'
import { useAuth } from '../../contexts/AuthContext'
import { formatDate, formatPrice } from '../../utils/formatters'
import { Calendar, MapPin, Users, ArrowLeft } from 'lucide-react'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'

const TourDetail = () => {
  const { id } = useParams()
  const navigate = useNavigate()
  const { isAuthenticated } = useAuth()
  const queryClient = useQueryClient()

  const { data: tour, isLoading, error } = useQuery(
    ['tour', id],
    () => tourAPI.getById(id),
    {
      select: (res) => res.data
    }
  )

  const bookingMutation = useMutation(
    (tourId) => tourBookingAPI.create({ tourId, status: 'Pending' }),
    {
      onSuccess: () => {
        queryClient.invalidateQueries(['user-tour-bookings'])
        navigate('/profile')
      },
      onError: () => {
        alert('Booking error occurred')
      }
    }
  )

  const handleBooking = () => {
    if (!isAuthenticated) {
      navigate('/login')
      return
    }
    bookingMutation.mutate(tour.id)
  }

  if (isLoading) return <LoadingSpinner />
  if (error) return <ErrorMessage message="Failed to load tour" />
  if (!tour) return <ErrorMessage message="Tour not found" />

  return (
    <div className="max-w-4xl mx-auto px-4">
      <button
        onClick={() => navigate(-1)}
        className="flex items-center space-x-2 text-gray-600 hover:text-blue-600 mb-6 transition-colors"
        aria-label="Go back"
      >
        <ArrowLeft className="h-5 w-5" />
        <span>Back</span>
      </button>

      <div className="bg-white rounded-lg shadow-md p-6">
        {tour.imageUrl && (
          <img
            src={tour.imageUrl}
            alt={tour.name}
            className="w-full h-64 object-cover rounded-lg mb-6"
          />
        )}

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-2">
            <h1 className="text-3xl font-bold mb-4 text-gray-900">{tour.name}</h1>

            <div className="space-y-4 mb-6 text-gray-700">
              <div className="flex items-center space-x-3">
                <MapPin className="h-5 w-5 text-gray-500" />
                <span className="text-lg">{tour.country}, {tour.region}</span>
              </div>

              <div className="flex items-center space-x-3">
                <Calendar className="h-5 w-5 text-gray-500" />
                <span className="text-lg">
                  {formatDate(tour.startDate)} - {formatDate(tour.endDate)}
                </span>
              </div>

              <div className="flex items-center space-x-3">
                <Users className="h-5 w-5 text-gray-500" />
                <span className="inline-block bg-blue-100 text-blue-800 text-sm font-medium px-3 py-1 rounded-full">
                  {tour.type}
                </span>
              </div>
            </div>
          </div>

          <div className="lg:col-span-1 bg-gray-50 p-6 rounded-lg flex flex-col justify-center">
            <div className="text-center mb-6">
              <span className="text-3xl font-bold text-green-600">{formatPrice(tour.price)}</span>
              <p className="text-gray-600">per person</p>
            </div>

            <button
              onClick={handleBooking}
              disabled={bookingMutation.isLoading}
              className="w-full bg-blue-600 text-white py-3 rounded-md hover:bg-blue-700 disabled:opacity-50 transition"
            >
              {bookingMutation.isLoading ? 'Booking...' : 'Book Tour'}
            </button>

            {!isAuthenticated && (
              <p className="text-sm text-gray-500 text-center mt-2">
                Please log in to book this tour
              </p>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}

export default TourDetail