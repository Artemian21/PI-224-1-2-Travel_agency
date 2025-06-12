import React from 'react'
import { Plus, Edit, Trash2, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'
import DeleteConfirmationModal from '../../UI/DeleteConfirmationModal'
import { useManageRooms } from '../../hooks/useManageRooms'

const RoomManagement = () => {
  const {
    hotels,
    form,
    filteredRooms,
    searchTerm,
    setSearchTerm,
    isEditing,
    onEdit,
    onCancel,
    deleteRoomId,
    setDeleteRoomId,
    confirmDelete,
    cancelDelete,
    handleChange,
    handleSubmit,
    saveRoomLoading,
    deleteRoomLoading,
  } = useManageRooms()

  return (
    <div className="space-y-6 relative">
      <div className="flex justify-between items-center">
        <h2 className="text-2xl font-semibold">Manage Rooms</h2>
        <button
          onClick={() => onEdit({})}
          className="btn-primary flex items-center gap-2"
          aria-label="Add new room"
        >
          <Plus className="h-5 w-5" />
          <span>Add New Room</span>
        </button>
      </div>

      <input
        type="search"
        placeholder="Search rooms or hotels..."
        className="form-input w-full max-w-md rounded-md border border-gray-300 px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        aria-label="Search rooms or hotels"
      />

      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Room Type</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Hotel</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Capacity</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Price Per Night</th>
              <th className="px-6 py-3 text-left text-xs font-semibold text-gray-500 uppercase tracking-wide">Actions</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {filteredRooms?.map((room) => (
              <tr key={room.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{room.roomType}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{room.hotelName ? `${room.hotelName}, ${room.hotelAddress}` : 'N/A'}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{room.capacity}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">${room.pricePerNight.toFixed(2)}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <div className="flex space-x-4">
                    <Link
                      to={`/rooms/${room.id}`}
                      className="text-blue-600 hover:text-blue-900"
                      aria-label={`View details of room ${room.roomType}`}
                    >
                      <Eye className="h-5 w-5" />
                    </Link>
                    <button
                      onClick={() => onEdit(room)}
                      className="text-green-600 hover:text-green-900"
                      aria-label={`Edit room ${room.roomType}`}
                    >
                      <Edit className="h-5 w-5" />
                    </button>
                    <button
                      onClick={() => setDeleteRoomId(room.id)}
                      disabled={deleteRoomLoading}
                      className={`text-red-600 hover:text-red-900 ${deleteRoomLoading ? 'opacity-50 cursor-not-allowed' : ''}`}
                      aria-label={`Delete room ${room.roomType}`}
                    >
                      <Trash2 className="h-5 w-5" />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {deleteRoomId && (
        <DeleteConfirmationModal onConfirm={confirmDelete} onCancel={cancelDelete}>
          Are you sure you want to delete this room?
        </DeleteConfirmationModal>
      )}

      {isEditing && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
          onClick={onCancel}
          role="dialog"
          aria-modal="true"
          aria-labelledby="modal-title"
        >
          <div className="bg-white rounded-lg p-6 max-w-md w-full" onClick={(e) => e.stopPropagation()}>
            <h3 id="modal-title" className="text-lg font-semibold mb-5">
              {form.id ? 'Edit Room' : 'Create New Room'}
            </h3>
            <form onSubmit={handleSubmit} className="space-y-5">
              <label className="block">
                <span className="text-gray-700 font-medium mb-1 block">Hotel</span>
                <select
                  name="hotelId"
                  value={form.hotelId}
                  onChange={handleChange}
                  required
                  className="w-full rounded-md border border-gray-300 p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="">Select hotel</option>
                  {hotels.map((hotel) => (
                    <option key={hotel.id} value={hotel.id}>
                      {hotel.name} — {hotel.address}
                    </option>
                  ))}
                </select>
              </label>

              <label className="block">
                <span className="text-gray-700 font-medium mb-1 block">Room Type</span>
                <select
                  name="roomType"
                  value={form.roomType}
                  onChange={handleChange}
                  required
                  className="w-full rounded-md border border-gray-300 p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                  <option value="Single">Single</option>
                  <option value="Double">Double</option>
                  <option value="Triple">Triple</option>
                  <option value="Family">Family</option>
                  <option value="Suite">Suite</option>
                </select>
              </label>

              <label className="block">
                <span className="text-gray-700 font-medium mb-1 block">Capacity</span>
                <input
                  type="number"
                  name="capacity"
                  value={form.capacity}
                  onChange={handleChange}
                  required
                  min={1}
                  className="w-full rounded-md border border-gray-300 p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </label>

              <label className="block">
                <span className="text-gray-700 font-medium mb-1 block">Price Per Night</span>
                <input
                  type="number"
                  name="pricePerNight"
                  value={form.pricePerNight}
                  onChange={handleChange}
                  required
                  min={0}
                  step="0.01"
                  className="w-full rounded-md border border-gray-300 p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </label>

              <div className="flex justify-end space-x-3">
                <button type="button" onClick={onCancel} className="btn-secondary">
                  Cancel
                </button>
                <button type="submit" disabled={saveRoomLoading} className="btn-primary">
                  {saveRoomLoading ? 'Saving...' : 'Save'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default RoomManagement