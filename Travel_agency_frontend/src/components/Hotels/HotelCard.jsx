import React from 'react'
import { Link } from 'react-router-dom'
import { MapPin } from 'lucide-react'

const HotelCard = ({ hotel }) => {
  return (
    <div className="rounded-lg shadow-md hover:shadow-xl transition-shadow bg-white p-6 flex flex-col justify-between">
      <div className="space-y-3">
        <h3 className="text-2xl font-semibold text-gray-900">{hotel.name}</h3>

        <div className="flex items-center space-x-2 text-gray-500">
          <MapPin className="h-5 w-5" />
          <span>{hotel.country}, {hotel.city}</span>
        </div>

        <p className="text-gray-600">{hotel.address}</p>
      </div>

      <Link
        to={`/hotels/${hotel.id}`}
        className="mt-6 inline-block bg-blue-600 text-white font-semibold px-5 py-3 rounded-lg text-center hover:bg-blue-700 transition-colors"
      >
        View Rooms
      </Link>
    </div>
  )
}

export default HotelCard