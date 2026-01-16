import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreLogStatusesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreLogStatuses(input: GetAdvPreLogStatusesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreLogStatuses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-log-status", {
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

export function useAllAdvPreLogStatuses() {
  return useQuery({
    queryKey: ["advPreLogStatuses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-log-status", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreLogStatus(id: string) {
  return useQuery({
    queryKey: ["advPreLogStatus", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-log-status/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreLogStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-log-status", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreLogStatuses"] });
    },
  });
}

export function useUpdateAdvPreLogStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-log-status/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreLogStatuses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreLogStatus", data.id] });
    },
  });
}

export function useDeleteAdvPreLogStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-log-status/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreLogStatuses"] });
    },
  });
}


