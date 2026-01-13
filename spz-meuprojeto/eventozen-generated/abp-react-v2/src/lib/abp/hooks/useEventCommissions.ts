import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetEventCommissionsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  eventId?: string;
  }

export function useEventCommissions(input: GetEventCommissionsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["eventCommissions", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/event-commission", {
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

export function useAllEventCommissions() {
  return useQuery({
    queryKey: ["eventCommissions", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/event-commission", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useEventCommission(id: string) {
  return useQuery({
    queryKey: ["eventCommission", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/event-commission/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateEventCommission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/event-commission", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["eventCommissions"] });
    },
  });
}

export function useUpdateEventCommission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/event-commission/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["eventCommissions"] });
      queryClient.invalidateQueries({ queryKey: ["eventCommission", data.id] });
    },
  });
}

export function useDeleteEventCommission() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/event-commission/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["eventCommissions"] });
    },
  });
}


