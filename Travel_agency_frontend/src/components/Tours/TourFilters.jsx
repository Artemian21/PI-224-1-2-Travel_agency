import React, { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { Search, Filter } from 'lucide-react'
import { TypeTour } from '../../constants/enums'

const TourFilters = ({ onFiltersChange, onSearchChange, initialFilters = {}, currentSearch='' }) => {
  const { register, handleSubmit, reset } = useForm()
  const [searchTerm, setSearchTerm] = useState('')

  useEffect(() => {
    reset(initialFilters)
    setSearchTerm(currentSearch)
  }, [initialFilters, currentSearch, reset])

  const onSubmit = data => {
    const f = {}
  
    if (data.country?.trim()) f.country = data.country.trim()
    if (data.region?.trim()) f.region = data.region.trim()
    if (data.type) f.type = data.type
    if (data.startDateFrom) f.startDateFrom = data.startDateFrom
    if (data.startDateTo) f.startDateTo = data.startDateTo
    if (data.price) f.price = parseFloat(data.price)
    if (data.name?.trim()) f.name = data.name.trim()
  
    if (Object.keys(f).length === 0) {
      onFiltersChange({})
      reset({})
    } else {
      onFiltersChange(f)
    }
  }

  const handleSearch = e => {
    e.preventDefault()
    onSearchChange(searchTerm.trim())
  }

  const clearAll = () => {
    reset({
      country: '',
      region: '',
      type: '',
      startDateFrom: '',
      startDateTo: '',
      price: '',
      name: ''
    });
    setSearchTerm('');
    onFiltersChange({});
    onSearchChange('');
  }

  return (
    <div className="rounded-xl shadow-md p-6 bg-white space-y-6">
      <h3 className="text-xl font-semibold flex items-center gap-2 text-gray-800">
        <Filter className="w-5 h-5 text-blue-600" /> Filters
      </h3>

      <form onSubmit={handleSearch}>
        <div className="flex">
          <input
            type="text"
            placeholder="Search tours..."
            className="form-input w-full rounded-l-md border-gray-300"
            value={searchTerm}
            onChange={e => setSearchTerm(e.target.value)}
          />
          <button type="submit" className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-r-md">
            <Search className="h-4 w-4" />
          </button>
        </div>
      </form>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <div>
          <label className="block text-sm font-medium mb-1">Country</label>
          <input type="text" className="form-input w-full" {...register('country')} placeholder="e.g. Turkey" />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Region</label>
          <input type="text" className="form-input w-full" {...register('region')} placeholder="e.g. Antalya" />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Tour Type</label>
          <select className="form-input w-full" {...register('type')}>
            <option value="">All types</option>
            {Object.values(TypeTour).map(t => <option key={t} value={t}>{t}</option>)}
          </select>
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Start Date (from)</label>
          <input type="date" className="form-input w-full" {...register('startDateFrom')} />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Start Date (to)</label>
          <input type="date" className="form-input w-full" {...register('startDateTo')} />
        </div>
        <div>
          <label className="block text-sm font-medium mb-1">Max Price</label>
          <input type="number" className="form-input w-full" {...register('price')} placeholder="0" />
        </div>

        <div className="flex gap-3">
          <button type="submit" className="w-full bg-blue-600 text-white py-2 rounded-md">Apply Filters</button>
          <button type="button" onClick={clearAll} className="w-full bg-gray-200 text-gray-800 py-2 rounded-md">Clear All</button>
        </div>
      </form>
    </div>
  )
}

export default TourFilters