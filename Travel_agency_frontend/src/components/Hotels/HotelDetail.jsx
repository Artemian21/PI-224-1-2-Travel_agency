import React from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useQuery } from 'react-query'
import { hotelAPI, hotelRoomAPI } from '../../services/api'
import { MapPin, ArrowLeft } from 'lucide-react'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import RoomCard from './RoomCard'

const HotelDetail = () => {
  const { id } = useParams()
  const navigate = useNavigate()

  const { data: hotel, isLoading: hotelLoading } = useQuery(
    ['hotel', id],
    () => hotelAPI.getById(id),
    {
      select: (data) => data.data
    }
  )

  const { data: rooms, isLoading: roomsLoading } = useQuery(
    ['hotel-rooms', id],
    () => hotelRoomAPI.getByHotelId(id),
    {
      select: (data) => data.data
    }
  )

  const handleRoomClick = (roomId) => {
    navigate(`/rooms/${roomId}`)
  }

  if (hotelLoading || roomsLoading) return <LoadingSpinner />
  if (!hotel) return <ErrorMessage message="Hotel not found" />

  return (
    <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
      <button
        onClick={() => navigate(-1)}
        className="flex items-center space-x-2 text-gray-600 hover:text-blue-600 mb-8"
        aria-label="Go back"
      >
        <ArrowLeft className="h-5 w-5" />
        <span>Back</span>
      </button>

      <div className="bg-white rounded-lg shadow-md p-8 mb-10">
        <h1 className="text-4xl font-extrabold mb-5 text-gray-900">{hotel.name}</h1>
        <div className="flex items-center space-x-3 text-gray-600 mb-5">
          <MapPin className="h-6 w-6" />
          <span className="text-lg">{hotel.country}, {hotel.city}</span>
        </div>
        <p className="text-gray-700 text-lg">{hotel.address}</p>
      </div>

      <section className="space-y-8">
        <h2 className="text-3xl font-bold text-gray-900">Available Rooms</h2>

        {rooms?.length === 0 && (
          <div className="text-center py-12 text-gray-500 text-lg">
            No rooms found
          </div>
        )}

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {rooms?.map(room => (
            <div
              key={room.id}
              onClick={() => handleRoomClick(room.id)}
              className="cursor-pointer transition-transform hover:scale-[1.02]"
              role="button"
              tabIndex={0}
              onKeyDown={e => (e.key === 'Enter' ? handleRoomClick(room.id) : null)}
              aria-label={`View details for room ${room.name}`}
            >
              <RoomCard room={room} hotelName={hotel.name} />
            </div>
          ))}
        </div>
      </section>
    </div>
  )
}

export default HotelDetail