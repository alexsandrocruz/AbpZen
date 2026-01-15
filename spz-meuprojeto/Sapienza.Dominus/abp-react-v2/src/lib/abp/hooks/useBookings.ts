import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetBookingsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  schedulerTypeId?: string;
  clientId?: string;
  }

export function useBookings(input: GetBookingsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["bookings", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/booking", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllBookings() {
  return useQuery({
    queryKey: ["bookings", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/booking", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useBooking(id: string) {
  return useQuery({
    queryKey: ["booking", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/booking/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateBooking() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/booking", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
  });
}

export function useUpdateBooking() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/booking/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
      queryClient.invalidateQueries({ queryKey: ["booking", data.id] });
    },
  });
}

export function useDeleteBooking() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/booking/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: SchedulerType <-> Client
 * Given a SchedulerType, toggle a Client
 */
export function useToggleClient(schedulerTypeId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateBooking();
  const deleteMutation = useDeleteBooking();
  const { data: existing } = useBookings({ schedulerTypeId: schedulerTypeId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ clientId, isChecked }: { clientId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.clientId === clientId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          schedulerTypeId: schedulerTypeId,
          clientId: clientId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: SchedulerType <-> Client
 * Given a Client, toggle a SchedulerType
 */
export function useToggleSchedulerType(clientId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateBooking();
  const deleteMutation = useDeleteBooking();
  const { data: existing } = useBookings({ clientId: clientId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ schedulerTypeId, isChecked }: { schedulerTypeId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.schedulerTypeId === schedulerTypeId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          schedulerTypeId: schedulerTypeId,
          clientId: clientId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
  });
}

