import React, { useState } from 'react'
import { Tab } from '@headlessui/react'
import TourManagement from './management/TourManagement'
import BookingManagement from './management/BookingManagement'
import HotelManagement from './management/HotelManagement'
import RoomManagement from './management/RoomManagement'
import TransportManagement from './management/TransportManagement'

const ManagerPanel = () => {
  const [activeTab, setActiveTab] = useState(0)

  const tabs = [
    { name: 'Bookings', component: <BookingManagement /> },
    { name: 'Tours', component: <TourManagement /> },
    { name: 'Hotels', component: <HotelManagement /> },
    { name: 'Rooms', component: <RoomManagement /> },
    { name: 'Transport', component: <TransportManagement /> }
  ]

  return (
    <div className="space-y-6 max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <h1 className="text-3xl font-extrabold text-gray-900">Manager Panel</h1>

      <Tab.Group selectedIndex={activeTab} onChange={setActiveTab}>
        <Tab.List className="flex space-x-2 rounded-lg bg-gray-50 p-1 shadow-inner">
          {tabs.map((tab, idx) => (
            <Tab
              key={idx}
              className={({ selected }) =>
                `flex-1 text-center rounded-md py-2 text-sm font-semibold transition-colors duration-200
                ${selected
                  ? 'bg-white shadow-md text-blue-600'
                  : 'text-gray-600 hover:bg-white hover:text-blue-500'}`
              }
            >
              {tab.name}
            </Tab>
          ))}
        </Tab.List>

        <Tab.Panels className="mt-4">
          {tabs.map((tab, idx) => (
            <Tab.Panel
              key={idx}
              className="rounded-lg bg-white p-6 shadow-md border border-gray-200"
            >
              {tab.component}
            </Tab.Panel>
          ))}
        </Tab.Panels>
      </Tab.Group>
    </div>
  )
}

export default ManagerPanel