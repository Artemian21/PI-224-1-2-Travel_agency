import { Link } from 'react-router-dom'

export default function ConflictPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-red-100 via-white to-red-100 px-4">
      <div className="text-center animate-fade-in-up">
        <h1 className="text-6xl font-extrabold text-red-600 drop-shadow mb-4">
          409
        </h1>
        <h2 className="text-2xl font-semibold text-gray-800">
          Conflict Detected
        </h2>
        <p className="mt-4 text-gray-600">
          There was a conflict with your request. The resource might already exist.
        </p>
        <Link
          to="/"
          className="inline-block mt-6 px-6 py-3 text-white bg-red-600 hover:bg-red-700 rounded-lg shadow transition"
        >
          Back to Home
        </Link>
      </div>
    </div>
  )
}
