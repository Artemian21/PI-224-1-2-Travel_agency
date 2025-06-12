import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { userAPI, hotelAPI, transportAPI, tourAPI, hotelRoomAPI } from '../../services/api';
import { User } from 'lucide-react';

export default function UserDetails() {
  const { userId } = useParams();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchUserDetails() {
      setLoading(true);
      try {
        const res = await userAPI.getById(userId);
        const userData = res.data;

        if (userData.hotelBookings?.length) {
          const hotelDetails = await Promise.all(
            userData.hotelBookings.map(async (booking) => {
              try {
                const roomRes = await hotelRoomAPI.getById(booking.hotelRoomId);
                const roomData = roomRes.data;
                if (!roomData?.hotelId) throw new Error('Missing hotelId in room');

                const hotelRes = await hotelAPI.getById(roomData.hotelId);
                return {
                  hotelName: hotelRes.data?.name || 'Unknown hotel',
                  roomInfo: roomData,
                };
              } catch {
                return {
                  hotelName: 'Unknown hotel',
                  roomInfo: null,
                };
              }
            })
          );
          userData.hotelBookings = userData.hotelBookings.map((b, i) => ({
            ...b,
            hotelName: hotelDetails[i].hotelName,
            roomInfo: hotelDetails[i].roomInfo,
          }));
        }

        if (userData.ticketBookings?.length) {
          const transportDetails = await Promise.all(
            userData.ticketBookings.map(async (booking) => {
              try {
                const transportRes = await transportAPI.getById(booking.transportId);
                return transportRes.data || null;
              } catch {
                return null;
              }
            })
          );
          userData.ticketBookings = userData.ticketBookings.map((b, i) => ({
            ...b,
            transportDetails: transportDetails[i],
          }));
        }

        if (userData.tourBookings?.length) {
          const tourDetails = await Promise.all(
            userData.tourBookings.map(async (booking) => {
              try {
                const tourRes = await tourAPI.getById(booking.tourId);
                return tourRes.data || null;
              } catch {
                return null;
              }
            })
          );
          userData.tourBookings = userData.tourBookings.map((b, i) => ({
            ...b,
            tourDetails: tourDetails[i],
          }));
        }

        setUser(userData);
        setError(null);
      } catch {
        setError('Failed to load user details or bookings.');
        setUser(null);
      } finally {
        setLoading(false);
      }
    }

    fetchUserDetails();
  }, [userId]);

  if (loading) return <div className="text-center py-8">Loading...</div>;
  if (error) return <div className="text-red-600 text-center py-8">{error}</div>;
  if (!user) return <div className="text-center py-8">User not found</div>;

  return (
    <div className="max-w-5xl mx-auto p-6 bg-white shadow-lg rounded-2xl space-y-10">
      <section className="flex items-center gap-6 border-b pb-6">
        <div className="bg-blue-600 p-4 rounded-full">
          <User className="text-white w-8 h-8" />
        </div>
        <div>
          <h2 className="text-3xl font-bold text-gray-900">{user.username}</h2>
          <p className="text-gray-600 text-lg">{user.email}</p>
          <span className="mt-2 inline-block bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-medium">
            {user.role}
          </span>
        </div>
      </section>

      <section>
        <h3 className="text-2xl font-semibold text-gray-800 mb-4">Hotel Bookings</h3>
        {user.hotelBookings?.length ? (
          <ul className="space-y-4">
            {user.hotelBookings.map((booking) => (
              <li
                key={booking.id}
                className="bg-gray-50 border border-gray-200 rounded-xl p-5 hover:shadow transition"
              >
                <p className="text-gray-700">
                  <strong>ID:</strong> <span className="font-mono text-sm">{booking.id}</span>
                </p>
                <p className="text-gray-700">
                  <strong>Hotel:</strong>{' '}
                  <span className="font-medium text-blue-700">{booking.hotelName}</span>
                </p>
                <p className="text-gray-700">
                  <strong>Check-in:</strong> {new Date(booking.startDate).toLocaleDateString()}
                </p>
                <p className="text-gray-700">
                  <strong>Check-out:</strong> {new Date(booking.endDate).toLocaleDateString()}
                </p>
                <p className="text-gray-700">
                  <strong>Guests:</strong> {booking.numberOfGuests}
                </p>
                <p className="text-gray-700">
                  <strong>Status:</strong>{' '}
                  <span
                    className={`font-semibold ${
                      booking.status === 'Completed'
                        ? 'text-green-600'
                        : booking.status === 'Pending'
                        ? 'text-orange-600'
                        : 'text-red-600'
                    }`}
                  >
                    {booking.status}
                  </span>
                </p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500 italic">No hotel bookings available.</p>
        )}
      </section>

      <hr className="border-gray-200" />

      <section>
        <h3 className="text-2xl font-semibold text-gray-800 mb-4">Transport Bookings</h3>
        {user.ticketBookings?.length ? (
          <ul className="space-y-4">
            {user.ticketBookings.map((booking) => (
              <li
                key={booking.id}
                className="bg-gray-50 border border-gray-200 rounded-xl p-5 hover:shadow transition"
              >
                <p className="text-gray-700">
                  <strong>ID:</strong> <span className="font-mono text-sm">{booking.id}</span>
                </p>
                {booking.transportDetails && (
                  <>
                    <p className="text-gray-700">
                      <strong>Type:</strong> {booking.transportDetails.type || 'Unknown'}
                    </p>
                    <p className="text-gray-700">
                      <strong>Company:</strong> {booking.transportDetails.company || 'Unknown'}
                    </p>
                    <p className="text-gray-700">
                      <strong>Price:</strong> $
                      {booking.transportDetails.price?.toFixed(2) ?? 'N/A'}
                    </p>
                  </>
                )}
                <p className="text-gray-700">
                  <strong>Booking Date:</strong>{' '}
                  {new Date(booking.bookingDate).toLocaleString()}
                </p>
                <p className="text-gray-700">
                  <strong>Status:</strong>{' '}
                  <span
                    className={`font-semibold ${
                      booking.status === 'Paid'
                        ? 'text-green-600'
                        : booking.status === 'Pending'
                        ? 'text-orange-600'
                        : 'text-red-600'
                    }`}
                  >
                    {booking.status}
                  </span>
                </p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500 italic">No transport bookings found.</p>
        )}
      </section>

      <hr className="border-gray-200" />

      <section>
        <h3 className="text-2xl font-semibold text-gray-800 mb-4">Tour Bookings</h3>
        {user.tourBookings?.length ? (
          <ul className="space-y-4">
            {user.tourBookings.map((booking) => (
              <li
                key={booking.id}
                className="bg-gray-50 border border-gray-200 rounded-xl p-5 hover:shadow transition"
              >
                <p className="text-gray-700">
                  <strong>ID:</strong> <span className="font-mono text-sm">{booking.id}</span>
                </p>
                {booking.tourDetails && (
                  <>
                    <p className="text-gray-700">
                      <strong>Tour Name:</strong> {booking.tourDetails.name || 'Unknown tour'}
                    </p>
                    <p className="text-gray-700">
                      <strong>Destination:</strong>{' '}
                      {booking.tourDetails.country && booking.tourDetails.region
                        ? `${booking.tourDetails.country}, ${booking.tourDetails.region}`
                        : 'Unknown'}
                    </p>
                    <p className="text-gray-700">
                      <strong>Start Date:</strong>{' '}
                      {new Date(booking.tourDetails.startDate).toLocaleDateString()}
                    </p>
                    <p className="text-gray-700">
                      <strong>End Date:</strong>{' '}
                      {new Date(booking.tourDetails.endDate).toLocaleDateString()}
                    </p>
                  </>
                )}
                <p className="text-gray-700">
                  <strong>Status:</strong>{' '}
                  <span
                    className={`font-semibold ${
                      booking.status === 'Confirmed'
                        ? 'text-green-600'
                        : booking.status === 'Pending'
                        ? 'text-orange-600'
                        : 'text-red-600'
                    }`}
                  >
                    {booking.status}
                  </span>
                </p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-500 italic">No tour bookings available.</p>
        )}
      </section>
    </div>
  );
}