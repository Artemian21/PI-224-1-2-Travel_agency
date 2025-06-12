import { useEffect, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from 'react-query'
import { tourAPI } from '../../services/api'

export const useManageTours = () => {
  const queryClient = useQueryClient()

  const [searchTerm, setSearchTerm] = useState('')
  const [editingTour, setEditingTour] = useState(null)
  const [form, setForm] = useState({
    name: '',
    type: 'HotDeal',
    country: '',
    region: '',
    startDate: '',
    endDate: '',
    price: 0,
    imageUrl: '',
  })

  const { data: tours, isLoading, error } = useQuery(['tours'], () => tourAPI.getAll(), {
    select: (data) => data.data,
  })

  const deleteMutation = useMutation((id) => tourAPI.delete(id), {
    onSuccess: () => queryClient.invalidateQueries(['tours']),
  })

  const createMutation = useMutation((newTour) => tourAPI.create(newTour), {
    onSuccess: () => {
      queryClient.invalidateQueries(['tours'])
      setEditingTour(null)
      resetForm()
    },
  })

  const updateMutation = useMutation(({ id, updatedTour }) => tourAPI.update(id, updatedTour), {
    onSuccess: () => {
      queryClient.invalidateQueries(['tours'])
      setEditingTour(null)
      resetForm()
    },
  })

  const [deleteTourId, setDeleteTourId] = useState(null)
    const handleDelete = (id) => {
        setDeleteTourId(id)
    }

    const confirmDelete = () => {
        if (deleteTourId) {
            deleteMutation.mutate(deleteTourId)
            setDeleteTourId(null)
        }
    }

    const cancelDelete = () => {
        setDeleteTourId(null)
    }

  const resetForm = () => {
    setForm({
      name: '',
      type: 'HotDeal',
      country: '',
      region: '',
      startDate: '',
      endDate: '',
      price: 0,
      imageUrl: '',
    })
  }

  const onChangeForm = (e) => {
    const { name, value } = e.target
    setForm((prev) => ({ ...prev, [name]: value }))
  }

  const onSubmit = (e) => {
    e.preventDefault()
    if (editingTour === 'new') {
      createMutation.mutate(form)
    } else if (editingTour && editingTour.id) {
      updateMutation.mutate({ id: editingTour.id, updatedTour: form })
    }
  }

  const onEdit = (tour) => {
    setEditingTour(tour)
    setForm({
      name: tour.name || '',
      type: tour.type || 'HotDeal',
      country: tour.country || '',
      region: tour.region || '',
      startDate: tour.startDate ? tour.startDate.slice(0, 10) : '',
      endDate: tour.endDate ? tour.endDate.slice(0, 10) : '',
      price: tour.price || 0,
      imageUrl: tour.imageUrl || '',
    })
  }

  const filteredTours = tours?.filter(
    (tour) =>
      tour.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      tour.country.toLowerCase().includes(searchTerm.toLowerCase())
  ) || []

  return {
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
    confirmDelete,
    cancelDelete,
    deleteTourId,
    resetForm,
    error,
    createOrUpdateLoading: createMutation.isLoading || updateMutation.isLoading,
    filteredTours,
  }
}
