import React, { useEffect, useState } from 'react'
import { transportAPI } from '../../services/api'

export const useManageTransport = () => {
  const [transports, setTransports] = useState([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const [formData, setFormData] = useState({
    id: null,
    type: '',
    company: '',
    departureDate: '',
    arrivalDate: '',
    price: '',
  })

  const [isEditing, setIsEditing] = useState(false)
  const [deleteTransportId, setDeleteTransportId] = useState(null)
  const [searchTerm, setSearchTerm] = useState('')

  const fetchTransports = async () => {
    setLoading(true)
    try {
      const response = await transportAPI.getAll()
      setTransports(response.data)
      setError(null)
    } catch (err) {
      setError('Помилка при завантаженні транспорту')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchTransports()
  }, [])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({ ...prev, [name]: value }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    const { id, type, company, departureDate, arrivalDate, price } = formData

    if (!type || !company || !departureDate || !arrivalDate || !price) {
      return
    }

    const transportData = {
      type,
      company,
      departureDate,
      arrivalDate,
      price: parseFloat(price),
    }


    if (id !== null && isEditing) {
      await transportAPI.update(id, transportData)
    } else {
      await transportAPI.create(transportData)
    }

    // Очистка
    setFormData({
      id: null,
      type: '',
      company: '',
      departureDate: '',
      arrivalDate: '',
      price: '',
    })
    setIsEditing(false)
    fetchTransports()

  }

  const handleEdit = (transport) => {
    if (transport === 'new') {
      setFormData({
        id: null,
        type: '',
        company: '',
        departureDate: '',
        arrivalDate: '',
        price: '',
      })
    } else {
      setFormData({
        id: transport.id,
        type: transport.type || '',
        company: transport.company || '',
        departureDate: transport.departureDate || '',
        arrivalDate: transport.arrivalDate || '',
        price: transport.price?.toString() || '',
      })
    }
    setIsEditing(true)
  }

  const handleDelete = async (id) => {
    try {
      await transportAPI.delete(id)
      fetchTransports()
    } catch {
      alert('Не вдалося видалити транспорт')
    }
  }

  const confirmDelete = () => {
    if (deleteTransportId) {
      handleDelete(deleteTransportId)
      setDeleteTransportId(null)
    }
  }

  const cancelDelete = () => setDeleteTransportId(null)

  const handleCancel = () => {
    setFormData({
      id: null,
      type: '',
      company: '',
      departureDate: '',
      arrivalDate: '',
      price: '',
    })
    setIsEditing(false)
  }

  const filteredTransports = transports?.filter((transport) => {
    const search = searchTerm.toLowerCase()

    const typeMatch = transport.type.toLowerCase().includes(search)
    const companyMatch = transport.company.toLowerCase().includes(search)

    const departureMatch = new Date(transport.departureDate)
      .toLocaleString('uk-UA', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
      })
      .toLowerCase()
      .includes(search)

    const arrivalMatch = new Date(transport.arrivalDate)
      .toLocaleString('uk-UA', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
      })
      .toLowerCase()
      .includes(search)

    const priceMatch = transport.price.toString().includes(search)

    return typeMatch || companyMatch || departureMatch || arrivalMatch || priceMatch
  })

  return {
    transports,
    loading,
    error,
    formData,
    isEditing,
    deleteTransportId,
    filteredTransports,
    searchTerm,
    setDeleteTransportId,
    confirmDelete,
    cancelDelete,
    setSearchTerm,
    handleChange,
    handleSubmit,
    handleEdit,
    handleCancel,
  }
}