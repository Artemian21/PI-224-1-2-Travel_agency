import React, { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { Search, Filter } from 'lucide-react'
import { TypeTour } from '../../constants/enums'

const TourFilters = ({ onFiltersChange, onSearchChange, initialFilters = {} }) => {
  const { register, handleSubmit, reset } = useForm()
  const [searchTerm, setSearchTerm] = useState('')

  useEffect(() => {
    reset(initialFilters)
  }, [initialFilters, reset])

  const onSubmit = (data) => {
    const filters = {}
  
    if (data.country?.trim()) filters.country = data.country.trim()
    if (data.type?.trim()) filters.type = data.type.trim()
    if (data.region?.trim()) filters.region = data.region.trim()
    if (data.startDateFrom) filters.startDateFrom = data.startDateFrom
    if (data.startDateTo) filters.startDateTo = data.startDateTo
    if (data.price) filters.price = parseFloat(data.price)
  
    onFiltersChange(filters)
  }

  const handleSearch = (e) => {
    e.preventDefault()
    onSearchChange(searchTerm)
  }

  const clearFilters = () => {
    reset()
    setSearchTerm('')
    onFiltersChange({})
    onSearchChange('')
  }

  return (
    <div className="rounded-xl shadow-md p-6 bg-white space-y-6">
      <h3 className="text-xl font-semibold flex items-center gap-2 text-gray-800">
        <Filter className="w-5 h-5 text-blue-600" />
        Filters
      </h3>

      <form onSubmit={handleSearch}>
        <div className="flex">
          <input
            type="text"
            placeholder="Search tours..."
            className="form-input w-full rounded-l-md border border-gray-300"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <button
            type="submit"
            className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-r-md"
          >
            <Search className="h-4 w-4" />
          </button>
        </div>
      </form>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Country</label>
          <input
            type="text"
            className="form-input w-full"
            placeholder="e.g. Turkey"
            {...register('country')}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Region</label>
          <input
            type="text"
            className="form-input w-full"
            placeholder="e.g. Antalya"
            {...register('region')}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Tour Type</label>
          <select className="form-input w-full" {...register('type')}>
            <option value="">All types</option>
            {Object.values(TypeTour).map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Start Date (from)</label>
          <input
            type="date"
            className="form-input w-full"
            {...register('startDateFrom')}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Start Date (to)</label>
          <input
            type="date"
            className="form-input w-full"
            {...register('startDateTo')}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Maximum Price</label>
          <input
            type="number"
            className="form-input w-full"
            placeholder="0"
            {...register('price')}
          />
        </div>

        <div className="flex gap-3">
          <button type="submit" className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-md transition">
            Apply
          </button>
          <button
            type="button"
            onClick={clearFilters}
            className="w-full bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium py-2 px-4 rounded-md transition"
          >
            Clear
          </button>
        </div>
      </form>
    </div>
  )
}

export default TourFilters