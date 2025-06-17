import React from 'react'
import { Link } from 'react-router-dom'
import { useQuery } from 'react-query'
import { tourAPI, hotelAPI } from '../../services/api'
import { Plane, Building, Map } from 'lucide-react'
import TourCard from '../Tours/TourCard'
import HotelCard from '../Hotels/HotelCard'
import LoadingSpinner from '../UI/LoadingSpinner'

const Home = () => {
  const { data: tours, isLoading: toursLoading } = useQuery(
    ['featured-tours'],
    () => tourAPI.getAll(),
    {
      select: (data) => data.data?.slice(0, 3) || []
    }
  )

  const { data: hotels, isLoading: hotelsLoading } = useQuery(
    ['featured-hotels'],
    () => hotelAPI.getAll(),
    {
      select: (data) => data.data?.slice(0, 3) || []
    }
  )

  return (
    <div className="space-y-20 px-6 md:px-12 lg:px-24">
      
      <section className="text-center py-20 bg-gradient-to-r from-blue-700 to-purple-700 text-white rounded-xl shadow-lg max-w-5xl mx-auto">
        <h1 className="text-5xl font-extrabold mb-6 drop-shadow-lg">Travel With Us!</h1>
        <p className="text-xl mb-10 max-w-3xl mx-auto">
          Discover the best tours, hotels, and transportation solutions worldwide.
        </p>
        <div className="flex flex-col sm:flex-row justify-center gap-6">
          <Link
            to="/tours"
            className="bg-white text-blue-700 font-semibold px-10 py-3 rounded-lg shadow-md hover:bg-gray-100 transition"
          >
            Explore Tours
          </Link>
          <Link
            to="/hotels"
            className="border-2 border-white text-white px-10 py-3 rounded-lg font-semibold hover:bg-white hover:text-blue-700 transition"
          >
            Find Hotels
          </Link>
        </div>
      </section>

      <section className="max-w-5xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-12 text-center">
        <div>
          <div className="w-20 h-20 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Map className="h-10 w-10 text-blue-600" />
          </div>
          <h3 className="text-xl font-semibold mb-2">Best Tours</h3>
          <p className="text-gray-600">Carefully selected routes to the most beautiful places around the world.</p>
        </div>

        <div>
          <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Building className="h-10 w-10 text-green-600" />
          </div>
          <h3 className="text-xl font-semibold mb-2">Comfortable Hotels</h3>
          <p className="text-gray-600">Verified hotels with exceptional service quality.</p>
        </div>

        <div>
          <div className="w-20 h-20 bg-purple-100 rounded-full flex items-center justify-center mx-auto mb-4">
            <Plane className="h-10 w-10 text-purple-600" />
          </div>
          <h3 className="text-xl font-semibold mb-2">Convenient Transportation</h3>
          <p className="text-gray-600">Reliable transport solutions for your journeys.</p>
        </div>
      </section>

      <section className="max-w-7xl mx-auto">
        <div className="flex items-center justify-between mb-8 px-4">
          <h2 className="text-3xl font-bold">Popular Tours</h2>
          <Link to="/tours" className="text-blue-700 hover:underline font-semibold">
            View All →
          </Link>
        </div>
        {toursLoading ? (
          <LoadingSpinner />
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 px-4">
            {tours?.map(tour => (
              <TourCard key={tour.id} tour={tour} />
            ))}
          </div>
        )}
      </section>

      <section className="max-w-7xl mx-auto">
        <div className="flex items-center justify-between mb-8 px-4">
          <h2 className="text-3xl font-bold">Recommended Hotels</h2>
          <Link to="/hotels" className="text-blue-700 hover:underline font-semibold">
            View All →
          </Link>
        </div>
        {hotelsLoading ? (
          <LoadingSpinner />
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 px-4">
            {hotels?.map(hotel => (
              <HotelCard key={hotel.id} hotel={hotel} />
            ))}
          </div>
        )}
      </section>

      <section className="bg-gray-100 rounded-xl p-16 text-center max-w-4xl mx-auto shadow-md">
        <h2 className="text-4xl font-bold mb-4">Ready to travel?</h2>
        <p className="text-lg text-gray-700 mb-10">
          Register now and get access to exclusive offers and deals.
        </p>
        <Link
          to="/register"
          className="inline-block bg-blue-700 text-white px-12 py-4 rounded-lg font-semibold shadow hover:bg-blue-800 transition"
        >
          Register Now
        </Link>
      </section>
    </div>
  )
}

export default Home