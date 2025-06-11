import { Link } from 'react-router-dom'

export default function ServerErrorPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-tr from-red-100 to-red-200 px-4">
      <div className="text-center animate-fade-in-up max-w-md">
        <h1 className="text-7xl font-extrabold text-red-600 drop-shadow mb-4">
          500
        </h1>
        <h2 className="text-3xl font-semibold text-gray-800 mb-2">
          Server Error
        </h2>
        <p className="text-gray-600 mb-6">
          An error occurred on the server. Please try again later.
        </p>
        <Link
          to="/"
          className="inline-block px-6 py-3 bg-red-600 text-white rounded-lg shadow-md hover:bg-red-700 transition"
        >
          Back to Home
        </Link>
      </div>
    </div>
  )
}