import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSchedulerAvailabilitiesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  schedulerTypeId?: string;
  }

export function useSchedulerAvailabilities(input: GetSchedulerAvailabilitiesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["schedulerAvailabilities", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-availability", {
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

export function useAllSchedulerAvailabilities() {
  return useQuery({
    queryKey: ["schedulerAvailabilities", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/scheduler-availability", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSchedulerAvailability(id: string) {
  return useQuery({
    queryKey: ["schedulerAvailability", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/scheduler-availability/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSchedulerAvailability() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/scheduler-availability", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerAvailabilities"] });
    },
  });
}

export function useUpdateSchedulerAvailability() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/scheduler-availability/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["schedulerAvailabilities"] });
      queryClient.invalidateQueries({ queryKey: ["schedulerAvailability", data.id] });
    },
  });
}

export function useDeleteSchedulerAvailability() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/scheduler-availability/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["schedulerAvailabilities"] });
    },
  });
}


