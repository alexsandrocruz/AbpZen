import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreStatusesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreStatuses(input: GetAdvPreStatusesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreStatuses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-status", {
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

export function useAllAdvPreStatuses() {
  return useQuery({
    queryKey: ["advPreStatuses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-status", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreStatus(id: string) {
  return useQuery({
    queryKey: ["advPreStatus", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-status/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-status", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatuses"] });
    },
  });
}

export function useUpdateAdvPreStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-status/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatuses"] });
      queryClient.invalidateQueries({ queryKey: ["advPreStatus", data.id] });
    },
  });
}

export function useDeleteAdvPreStatus() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-status/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreStatuses"] });
    },
  });
}


