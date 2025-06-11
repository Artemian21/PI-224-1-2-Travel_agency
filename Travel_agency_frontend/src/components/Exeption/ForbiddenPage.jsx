import { Link } from 'react-router-dom'

export default function ForbiddenPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-gray-100 to-gray-200 px-4">
      <div className="text-center animate-fade-in-up">
        <h1 className="text-7xl font-extrabold text-red-600 drop-shadow mb-4">
          403
        </h1>
        <h2 className="text-3xl font-semibold text-gray-800 mb-2">
          Forbidden
        </h2>
        <p className="text-gray-600 mb-6">
          You don’t have permission to access this page.
        </p>
        <Link
          to="/"
          className="inline-block px-6 py-3 text-white bg-red-600 hover:bg-red-700 rounded-lg shadow-md transition"
        >
          Back to Home
        </Link>
      </div>
    </div>
  )
}
