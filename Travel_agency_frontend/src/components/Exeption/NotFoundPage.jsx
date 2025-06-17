import { Link } from 'react-router-dom'

export default function NotFoundPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200 px-4">
      <div className="text-center animate-fade-in-up">
        <h1 className="text-7xl font-extrabold text-blue-600 drop-shadow mb-4">
          404
        </h1>
        <h2 className="text-3xl font-semibold text-gray-800 mb-2">
          Page Not Found
        </h2>
        <p className="text-gray-600 mb-6">
          The page you're looking for doesn't exist or has been moved.
        </p>
        <Link
          to="/"
          className="inline-block px-6 py-3 text-white bg-blue-600 hover:bg-blue-700 rounded-lg shadow-md transition"
        >
          Back to Home
        </Link>
      </div>
    </div>
  )
}