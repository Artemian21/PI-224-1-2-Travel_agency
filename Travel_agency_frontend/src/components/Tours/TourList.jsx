import React, { useState, useEffect } from 'react'
import { useQuery } from 'react-query'
import { tourAPI } from '../../services/api'
import TourCard from './TourCard'
import LoadingSpinner from '../UI/LoadingSpinner'
import ErrorMessage from '../UI/ErrorMessage'
import TourFilters from './TourFilters'

const pageSize = 10

const TourList = () => {
  const [filters, setFilters] = useState({})
  const [searchQuery, setSearchQuery] = useState('')
  const [page, setPage] = useState(1)
  const [allTours, setAllTours] = useState([])  // для зберігання всіх турів

  // Завантажуємо всі тури один раз для пошуку і фільтрації по всьому масиву
  useEffect(() => {
    const fetchAllTours = async () => {
      try {
        const res = await tourAPI.getAll()
        setAllTours(res.data)
      } catch (e) {
        setAllTours([])
      }
    }
    fetchAllTours()
  }, [])

  // Функція фільтрації локально (коли вже є всі тури)
  const filteredTours = React.useMemo(() => {
    let tours = allTours

    // Якщо пошук
    if (searchQuery.trim()) {
      const q = searchQuery.toLowerCase()
      tours = tours.filter(t => 
        t.name.toLowerCase().includes(q) ||
        (t.country && t.country.toLowerCase().includes(q)) ||
        (t.region && t.region.toLowerCase().includes(q)) ||
        (t.type && t.type.toLowerCase().includes(q))
      )
    } else if (Object.keys(filters).length > 0) {
      // Фільтрація за полями
      tours = tours.filter(t => {
        if (filters.country && (!t.country || !t.country.toLowerCase().includes(filters.country.toLowerCase())))
          return false
        if (filters.region && (!t.region || !t.region.toLowerCase().includes(filters.region.toLowerCase())))
          return false
        if (filters.type && t.type !== filters.type)
          return false
        if (filters.name && (!t.name || !t.name.toLowerCase().includes(filters.name.toLowerCase())))
          return false
        if (filters.price && t.price > filters.price)
          return false
        if (filters.startDateFrom && new Date(t.startDate) < new Date(filters.startDateFrom))
          return false
        if (filters.startDateTo && new Date(t.startDate) > new Date(filters.startDateTo))
          return false
        return true
      })
    }
    return tours
  }, [allTours, filters, searchQuery])

  // Пагінація локальна
  const pagedTours = React.useMemo(() => {
    const start = (page - 1) * pageSize
    return filteredTours.slice(start, start + pageSize)
  }, [filteredTours, page])

  const totalPages = Math.ceil(filteredTours.length / pageSize)

  const handlePageChange = (newPage) => {
    if (newPage > 0 && newPage <= totalPages) {
      setPage(newPage)
    }
  }

  // Вивід
  if (!allTours.length) return (
    <div className="flex justify-center items-center min-h-[50vh]">
      <LoadingSpinner />
    </div>
  )

  return (
    <div className="px-4 sm:px-6 lg:px-8 py-6 space-y-6">
      <div className="flex flex-col lg:flex-row gap-6">
        <aside className="lg:w-1/4 w-full">
          <TourFilters
            initialFilters={filters}
            currentSearch={searchQuery}
            onFiltersChange={(f) => {
              setFilters(f)
              setSearchQuery('')
              setPage(1)
            }}
            onSearchChange={(q) => {
              setSearchQuery(q)
              setFilters({})
              setPage(1)
            }}
          />
        </aside>

        <main className="lg:w-3/4 w-full">
          {(searchQuery || Object.keys(filters).length > 0) && (
            <div className="mb-4 p-4 bg-gray-100 rounded">
              <div className="text-sm text-gray-600 font-medium">Showing results for:</div>
              {searchQuery && <div>🔍 <strong>"{searchQuery}"</strong></div>}
              {Object.entries(filters).map(([k, v]) => (
                <div key={k}>✅ <strong className="capitalize">{k}</strong>: {String(v)}</div>
              ))}
            </div>
          )}

          {pagedTours.length > 0 ? (
            <>
              <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-6">
                {pagedTours.map(t => <TourCard key={t.id} tour={t} />)}
              </div>
              <div className="flex justify-center mt-8 gap-4 items-center">
                <button onClick={() => handlePageChange(page - 1)} disabled={page === 1} className="px-3 py-2 bg-gray-200 rounded disabled:opacity-50">Prev</button>
                <span>Page {page} of {totalPages}</span>
                <button onClick={() => handlePageChange(page + 1)} disabled={page === totalPages} className="px-3 py-2 bg-gray-200 rounded disabled:opacity-50">Next</button>
              </div>
            </>
          ) : (
            <div className="text-center text-gray-500 py-12 text-lg">No tours found.</div>
          )}
        </main>
      </div>
    </div>
  )
}

export default TourList