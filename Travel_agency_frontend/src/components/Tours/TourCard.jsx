import React from 'react'
import { Link } from 'react-router-dom'
import { Calendar, MapPin } from 'lucide-react'
import { formatDate, formatPrice } from '../../utils/formatters'

const TourCard = ({ tour }) => {
  return (
    <div className="rounded-lg bg-white shadow-md hover:shadow-lg transition-shadow duration-300 overflow-hidden">
      {tour.imageUrl && (
        <img
          src={tour.imageUrl}
          alt={tour.name}
          className="w-full h-48 object-cover"
        />
      )}
      <div className="p-4 space-y-3">
        <h3 className="text-xl font-semibold text-gray-900">{tour.name}</h3>
        <div className="flex items-center space-x-2 text-gray-600">
          <MapPin className="h-4 w-4" />
          <span>{tour.country}, {tour.region}</span>
        </div>
        <div className="flex items-center space-x-2 text-gray-600">
          <Calendar className="h-4 w-4" />
          <span>{formatDate(tour.startDate)} - {formatDate(tour.endDate)}</span>
        </div>
        <div>
          <span className="inline-block bg-blue-100 text-blue-800 text-sm font-medium px-3 py-1 rounded-full">
            {tour.type}
          </span>
        </div>
        <div className="flex items-center justify-between">
          <span className="text-2xl font-bold text-green-600">{formatPrice(tour.price)}</span>
          <Link
            to={`/tours/${tour.id}`}
            className="inline-block bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition"
          >
            Details
          </Link>
        </div>
      </div>
    </div>
  )
}

export default TourCard