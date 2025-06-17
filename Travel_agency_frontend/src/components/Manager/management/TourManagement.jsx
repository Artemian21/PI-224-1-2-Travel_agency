import React from 'react'
import { Plus, Edit, Trash2, Eye } from 'lucide-react'
import { Link } from 'react-router-dom'
import DeleteConfirmationModal from '../../UI/DeleteConfirmationModal'
import { useManageTours } from '../../hooks/useManageTours'

const TourManagement = () => {
  const {
    tours,
    searchTerm,
    setSearchTerm,
    editingTour,
    setEditingTour,
    form,
    onChangeForm,
    onSubmit,
    onEdit,
    handleDelete,
    deleteTourId,
    cancelDelete,
    confirmDelete,
    createOrUpdateLoading,
    resetForm,
    error,
    filteredTours,
  } = useManageTours()

  if (!tours) return <p>Loading...</p>
  if (error) return <p className="text-red-600">Failed to load tours</p>

  return (
    <div className="space-y-4 relative">
      <div className="flex justify-between items-center">
        <h2 className="text-xl font-semibold">Manage Tours</h2>
        <button
          onClick={() => {
            setEditingTour('new')
            resetForm()
          }}
          className="btn-primary flex items-center space-x-2"
          aria-label="Add new tour"
        >
          <Plus className="h-4 w-4" />
          <span>Add New Tour</span>
        </button>
      </div>

      <input
        type="text"
        placeholder="Search tours..."
        className="form-input w-full max-w-md"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        aria-label="Search tours"
      />

      <div className="overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Tour
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Country
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Price
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Duration
              </th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                Actions
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {filteredTours.map((tour) => (
              <tr key={tour.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap">
                  <div className="text-sm font-medium text-gray-900">{tour.name}</div>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{tour.country}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">${tour.price}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {tour.startDate && tour.endDate
                    ? `${Math.ceil(
                        (new Date(tour.endDate) - new Date(tour.startDate)) / (1000 * 60 * 60 * 24)
                      )} days`
                    : 'N/A'}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <div className="flex space-x-3">
                    <Link to={`/tours/${tour.id}`} className="text-blue-600 hover:text-blue-900" aria-label={`View details for ${tour.name}`}>
                      <Eye className="h-4 w-4" />
                    </Link>
                    <button
                      onClick={() => onEdit(tour)}
                      className="text-green-600 hover:text-green-900"
                      aria-label={`Edit ${tour.name}`}
                    >
                      <Edit className="h-4 w-4" />
                    </button>
                    <button
                      onClick={() => handleDelete(tour.id)}
                      className="text-red-600 hover:text-red-900"
                      aria-label={`Delete ${tour.name}`}
                    >
                      <Trash2 className="h-4 w-4" />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {deleteTourId && (
        <DeleteConfirmationModal onConfirm={confirmDelete} onCancel={cancelDelete}>
          Are you sure you want to delete this tour? This will also cancel all related bookings.
        </DeleteConfirmationModal>
      )}

      {(editingTour === 'new' || editingTour) && (
        <div
          className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
          onClick={() => {
            setEditingTour(null)
            resetForm()
          }}
          role="dialog"
          aria-modal="true"
          aria-labelledby="modal-title"
        >
          <div className="bg-white rounded-md p-6 max-w-md w-full" onClick={(e) => e.stopPropagation()}>
            <h3 id="modal-title" className="text-lg font-semibold mb-4">
              {editingTour === 'new' ? 'Create New Tour' : 'Edit Tour'}
            </h3>
            <form onSubmit={onSubmit} className="space-y-4">
              <input
                type="text"
                name="name"
                placeholder="Name"
                value={form.name}
                onChange={onChangeForm}
                required
                className="w-full p-2 border rounded"
                aria-label="Tour name"
              />
              <select
                name="type"
                value={form.type}
                onChange={onChangeForm}
                required
                className="w-full p-2 border rounded"
                aria-label="Tour type"
              >
                <option value="HotDeal">HotDeal</option>
                <option value="Excursion">Excursion</option>
                <option value="Beach">Beach</option>
                <option value="Ski">Ski</option>
                <option value="Adventure">Adventure</option>
                <option value="Cruise">Cruise</option>
                <option value="Wellness">Wellness</option>
                <option value="Shopping">Shopping</option>
                <option value="Cultural">Cultural</option>
                <option value="Safari">Safari</option>
                <option value="Eco">Eco</option>
                <option value="Family">Family</option>
                <option value="Romantic">Romantic</option>
                <option value="Extreme">Extreme</option>
                <option value="Festival">Festival</option>
                <option value="Business">Business</option>
              </select>
              <input
                type="text"
                name="country"
                placeholder="Country"
                value={form.country}
                onChange={onChangeForm}
                required
                className="w-full p-2 border rounded"
                aria-label="Country"
              />
              <input
                type="text"
                name="region"
                placeholder="Region"
                value={form.region}
                onChange={onChangeForm}
                className="w-full p-2 border rounded"
                aria-label="Region"
              />
              <label className="block">
                Start Date
                <input
                  type="date"
                  name="startDate"
                  value={form.startDate}
                  onChange={onChangeForm}
                  required
                  className="w-full p-2 border rounded"
                  aria-label="Start date"
                />
              </label>
              <label className="block">
                End Date
                <input
                  type="date"
                  name="endDate"
                  value={form.endDate}
                  onChange={onChangeForm}
                  required
                  className="w-full p-2 border rounded"
                  aria-label="End date"
                />
              </label>
              <input
                type="number"
                name="price"
                placeholder="Price"
                value={form.price}
                onChange={onChangeForm}
                required
                min={0}
                className="w-full p-2 border rounded"
                aria-label="Price"
              />
              <input
                type="text"
                name="imageUrl"
                placeholder="Image URL"
                value={form.imageUrl}
                onChange={onChangeForm}
                className="w-full p-2 border rounded"
                aria-label="Image URL"
              />

              <div className="flex justify-end space-x-3">
                <button
                  type="button"
                  onClick={() => {
                    setEditingTour(null)
                    resetForm()
                  }}
                  className="btn-secondary"
                >
                  Cancel
                </button>
                <button type="submit" disabled={createOrUpdateLoading} className="btn-primary">
                  {createOrUpdateLoading ? 'Saving...' : 'Save'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}

export default TourManagement