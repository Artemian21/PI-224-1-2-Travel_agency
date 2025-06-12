import React from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useQuery } from 'react-query'
import { hotelRoomAPI } from '../../services/api'
import { ArrowLeft, Users, Bed, MapPin } from 'lucide-react'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import { formatPrice } from '../../utils/formatters'

const RoomDetails = () => {
  const { id } = useParams()
  const navigate = useNavigate()

  const { data: room, isLoading, isError } = useQuery(
    ['room', id],
    () => hotelRoomAPI.getById(id),
    {
      select: (data) => data.data
    }
  )

  if (isLoading) return <LoadingSpinner />
  if (isError || !room) return <ErrorMessage message="Room not found" />

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-lg mt-10">
      <button
        onClick={() => navigate(-1)}
        className="flex items-center space-x-2 text-gray-500 hover:text-blue-600 transition-colors mb-6"
      >
        <ArrowLeft className="h-6 w-6" />
        <span className="text-base font-medium">Back</span>
      </button>

      <h1 className="text-4xl font-extrabold mb-6 text-gray-900">{room.roomType}</h1>

      <div className="flex flex-wrap items-center gap-6 mb-6 text-gray-700">
        <div className="flex items-center space-x-2">
          <Users className="h-6 w-6 text-blue-500" />
          <span className="text-lg">Up to {room.capacity} guests</span>
        </div>
        <div className="flex items-center space-x-2">
          <Bed className="h-6 w-6 text-green-500" />
          <span className="text-lg">{room.roomType}</span>
        </div>
      </div>

      <p className="text-2xl font-semibold text-green-700 mb-8">
        Price: {formatPrice(room.pricePerNight)} / night
      </p>

      {room.description && (
        <p className="text-gray-800 leading-relaxed mb-8">{room.description}</p>
      )}

      {room.latitude && room.longitude && (
        <div className="flex items-center space-x-3 text-gray-600 border border-gray-200 rounded-md p-4 shadow-sm">
          <MapPin className="h-5 w-5 text-red-500" />
          <span className="text-sm font-medium">
            Coordinates: {room.latitude.toFixed(4)}, {room.longitude.toFixed(4)}
          </span>
        </div>
      )}
    </div>
  )
}

export default RoomDetails