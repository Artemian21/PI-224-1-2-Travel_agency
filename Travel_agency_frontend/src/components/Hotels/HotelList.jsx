import React from 'react'
import { useQuery } from 'react-query'
import { hotelAPI } from '../../services/api'
import HotelCard from './HotelCard'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'

const HotelList = () => {
  const { data: hotels, isLoading, error } = useQuery(
    ['hotels'],
    () => hotelAPI.getAll(),
    {
      select: (data) => data.data
    }
  )

  if (isLoading) return <LoadingSpinner />
  if (error) return <ErrorMessage message="Failed to load hotels" />

  return (
    <section className="space-y-8 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <h1 className="text-4xl font-extrabold text-gray-900">Hotels</h1>

      {hotels?.length === 0 ? (
        <div className="text-center py-12 text-gray-500 text-lg">No hotels found</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {hotels.map((hotel) => (
            <HotelCard key={hotel.id} hotel={hotel} />
          ))}
        </div>
      )}
    </section>
  )
}

export default HotelList