import { Link } from 'react-router-dom'

export default function ValidationErrorPage() {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-tr from-yellow-100 to-yellow-200 px-4">
      <div className="text-center max-w-md animate-fade-in-up">
        <h1 className="text-7xl font-extrabold text-yellow-600 drop-shadow mb-4">
          400
        </h1>
        <h2 className="text-3xl font-semibold text-gray-800 mb-2">
          Validation Error
        </h2>
        <p className="text-gray-600 mb-6">
          The data you entered did not pass validation.
        </p>
        <Link
          to="/"
          className="inline-block px-6 py-3 bg-yellow-600 text-white rounded-lg shadow-md hover:bg-yellow-700 transition"
        >
          Back to Home
        </Link>
      </div>
    </div>
  )
}