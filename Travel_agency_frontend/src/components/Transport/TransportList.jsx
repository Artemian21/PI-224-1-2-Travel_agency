import React from 'react'
import { useQuery } from 'react-query'
import { transportAPI } from '../../services/api'
import TransportCard from './TransportCard'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'

const TransportList = () => {
  const { data: transports, isLoading, error } = useQuery(
    ['transports'],
    () => transportAPI.getAll(),
    {
      select: (data) => data.data
    }
  )

  if (isLoading) return <LoadingSpinner />
  if (error) return <ErrorMessage message="Failed to load transports" />

  return (
    <div className="max-w-7xl mx-auto px-4 py-8 space-y-6">
      <h1 className="text-4xl font-extrabold text-gray-900">Transport</h1>

      {transports?.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {transports.map((transport) => (
            <TransportCard key={transport.id} transport={transport} />
          ))}
        </div>
      ) : (
        <div className="text-center py-12 text-gray-500 text-lg">
          No transports found
        </div>
      )}
    </div>
  )
}

export default TransportList