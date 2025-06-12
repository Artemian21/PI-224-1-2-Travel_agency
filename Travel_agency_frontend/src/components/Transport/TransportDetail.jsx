import React from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { useQuery } from 'react-query'
import { transportAPI } from '../../services/api'
import { ArrowLeft, Plane, Building, Calendar } from 'lucide-react'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import { formatDateTime, formatPrice } from '../../utils/formatters'

const TransportDetail = () => {
  const { id } = useParams()
  const navigate = useNavigate()

  const { data: transport, isLoading, error } = useQuery(
    ['transport', id],
    () => transportAPI.getById(id),
    {
      select: (data) => data.data
    }
  )

  if (isLoading) return <LoadingSpinner />
  if (error || !transport) {
    return <ErrorMessage message="Transport not found" />
  }

  return (
    <div className="max-w-3xl mx-auto px-4 py-6">
      <button
        onClick={() => navigate(-1)}
        className="flex items-center gap-2 text-gray-600 hover:text-blue-600 mb-6"
      >
        <ArrowLeft className="h-5 w-5" />
        <span className="text-sm font-medium">Back</span>
      </button>

      <div className="rounded-2xl border bg-white shadow-sm p-6 space-y-6">
        <h1 className="text-3xl font-bold text-gray-800 flex items-center gap-2">
          <Plane className="h-6 w-6 text-blue-600" />
          {transport.type}
        </h1>

        <div className="flex items-center gap-2 text-gray-600">
          <Building className="h-5 w-5" />
          <span className="text-sm">{transport.company}</span>
        </div>

        <div className="space-y-2 text-sm text-gray-700">
          <div className="flex items-center gap-2">
            <Calendar className="h-5 w-5 text-gray-500" />
            <span>Departure: {formatDateTime(transport.departureDate)}</span>
          </div>
          <div className="flex items-center gap-2">
            <Calendar className="h-5 w-5 text-gray-500" />
            <span>Arrival: {formatDateTime(transport.arrivalDate)}</span>
          </div>
        </div>

        <div className="text-xl font-semibold text-green-600">
          Price: {formatPrice(transport.price)}
        </div>
      </div>
    </div>
  )
}

export default TransportDetail