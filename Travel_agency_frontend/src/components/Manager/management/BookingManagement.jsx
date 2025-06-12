import React from 'react'
import { Trash2 } from 'lucide-react'
import DeleteConfirmationModal from '../../UI/DeleteConfirmationModal'
import { useManageBookings } from '../../hooks/useManageBookings'
import LoadingSpinner from '../../UI/LoadingSpinner'

const BookingManagement = () => {
  const {
    usersData,
    searchTerm,
    setSearchTerm,
    statusFilter,
    setStatusFilter,
    typeFilter,
    setTypeFilter,
    selectedBooking,
    selectedUser,
    setSelectedBooking,
    setSelectedUser,
    filteredBookings,
    isLoading,
    handleStatusChange,
    handleDeleteBooking,
    openDetails,
    isDeleteModalOpen,
    confirmDelete,
    cancelDelete,
  } = useManageBookings()

  if (isLoading) return <LoadingSpinner />

  return (
    <div className="space-y-6">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between space-y-3 sm:space-y-0">
        <h2 className="text-2xl font-semibold text-gray-900">Manage Bookings</h2>
        <div className="flex flex-col sm:flex-row space-y-2 sm:space-y-0 sm:space-x-3 w-full sm:w-auto">
          <input
            type="text"
            placeholder="Search by user or type..."
            className="w-full sm:w-64 rounded-md border border-gray-300 px-3 py-2 text-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <select
            className="w-full sm:w-48 rounded-md border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
            value={statusFilter}
            onChange={(e) => setStatusFilter(e.target.value)}
          >
            <option value="All">All</option>
            <option value="Pending">Pending</option>
            <option value="Confirmed">Confirmed</option>
            <option value="Cancelled">Cancelled</option>
            <option value="Rejected">Rejected</option>
            <option value="Paid">Paid</option>
            <option value="Completed">Completed</option>
          </select>
          <select
            className="w-full sm:w-48 rounded-md border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
            value={typeFilter}
            onChange={(e) => setTypeFilter(e.target.value)}
          >
            <option value="All">All</option>
            <option value="Tour">Tour</option>
            <option value="Hotel">Hotel</option>
            <option value="Ticket">Ticket</option>
          </select>
        </div>
      </div>

      <div className="overflow-x-auto max-h-[500px] overflow-y-auto rounded-lg border border-gray-200 shadow-sm">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50 sticky top-0 z-10">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">User Email</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Type</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Status</th>
              <th className="px-6 py-3 text-center text-xs font-semibold text-gray-500 uppercase tracking-wide">Details</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Actions</th>
            </tr>
          </thead>
          <tbody>
            {filteredBookings.length === 0 ? (
              <tr>
                <td colSpan={5} className="text-center py-6 text-gray-500 italic">
                  No bookings found
                </td>
              </tr>
            ) : (
              filteredBookings.map((booking) => {
                const user = usersData.find((u) => u.id === booking.userId)
                return (
                  <tr key={booking.id} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-3 whitespace-nowrap text-sm text-gray-900">{user?.email || 'N/A'}</td>
                    <td className="px-6 py-3 whitespace-nowrap text-sm text-gray-700 capitalize">{booking.type}</td>
                    <td className="px-6 py-3 whitespace-nowrap text-sm">
                      <select
                        value={booking.status}
                        onChange={(e) => handleStatusChange(booking.type, booking.id, e.target.value)}
                        className="rounded-md border border-gray-300 px-2 py-1 text-sm text-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition"
                      >
                        <option value="Pending">Pending</option>
                        <option value="Confirmed">Confirmed</option>
                        <option value="Cancelled">Cancelled</option>
                        <option value="Rejected">Rejected</option>
                        <option value="Paid">Paid</option>
                        <option value="Completed">Completed</option>
                      </select>
                    </td>
                    <td className="px-6 py-3 whitespace-nowrap text-sm text-gray-500 text-center">
                      <button
                        onClick={() => openDetails(booking)}
                        className="text-blue-600 hover:text-blue-900 font-medium"
                        aria-label="View booking details"
                      >
                        View
                      </button>
                    </td>
                    <td className="px-6 py-3 whitespace-nowrap text-sm font-medium space-x-2">
                      <button
                        onClick={() => handleDeleteBooking(booking.type, booking.id)}
                        className="text-red-600 hover:text-red-900"
                        aria-label="Delete booking"
                      >
                        <Trash2 className="h-5 w-5" />
                      </button>
                    </td>
                  </tr>
                )
              })
            )}
          </tbody>
        </table>
      </div>

      {isDeleteModalOpen && (
        <DeleteConfirmationModal onConfirm={confirmDelete} onCancel={cancelDelete}>
          Are you sure you want to delete this booking?
        </DeleteConfirmationModal>
      )}

      {selectedBooking && (
        <BookingDetailsModal
          booking={selectedBooking}
          user={selectedUser}
          onClose={() => {
            setSelectedBooking(null)
            setSelectedUser(null)
          }}
        />
      )}
    </div>
  )
}

const BookingDetailsModal = ({ booking, user, onClose }) => {
  if (!booking) return null

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div className="bg-white p-6 rounded-lg shadow-lg max-w-md w-full">
        <h3 className="text-xl font-bold mb-4 text-gray-900">Booking Details</h3>
        <p className="mb-2"><strong>User Email:</strong> {user?.email || 'N/A'}</p>
        <p className="mb-2"><strong>Type:</strong> {booking.type}</p>
        <p className="mb-2"><strong>Status:</strong> {booking.status}</p>
        <button
          onClick={onClose}
          className="w-full py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition"
        >
          Close
        </button>
      </div>
    </div>
  )
}

export default BookingManagement