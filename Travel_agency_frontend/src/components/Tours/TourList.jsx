import React, { useState } from 'react'
import { useQuery } from 'react-query'
import { tourAPI } from '../../services/api'
import TourCard from './TourCard'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import TourFilters from './TourFilters'

const TourList = () => {
  const [filters, setFilters] = useState({})
  const [searchQuery, setSearchQuery] = useState('')

  const { data: tours, isLoading, error } = useQuery(
    ['tours', filters, searchQuery],
    () => {
      if (searchQuery) {
        return tourAPI.search(searchQuery)
      } else if (Object.keys(filters).length > 0) {
        return tourAPI.filter(filters)
      } else {
        return tourAPI.getAll()
      }
    },
    {
      select: (data) => data.data
    }
  )

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-[50vh]">
        <LoadingSpinner />
      </div>
    )
  }

  if (error) {
    return <ErrorMessage message="Failed to load tours." />
  }

  return (
    <div className="px-4 sm:px-6 lg:px-8 py-6 space-y-6">
      <div className="flex flex-col lg:flex-row gap-6">
        <aside className="lg:w-1/4 w-full">
          <TourFilters
            initialFilters={filters}
            onFiltersChange={setFilters}
            onSearchChange={setSearchQuery}
          />
        </aside>

        <main className="lg:w-3/4 w-full">
          {tours?.length > 0 ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-6">
              {tours.map((tour) => (
                <TourCard key={tour.id} tour={tour} />
              ))}
            </div>
          ) : (
            <div className="text-center text-gray-500 py-12 text-lg">
              No tours found.
            </div>
          )}
        </main>
      </div>
    </div>
  )
}

export default TourList