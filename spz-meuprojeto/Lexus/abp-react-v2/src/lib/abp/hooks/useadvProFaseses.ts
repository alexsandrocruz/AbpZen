import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProFasesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProFaseses(input: GetAdvProFasesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProFaseses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-fases", {
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

export function useAllAdvProFaseses() {
  return useQuery({
    queryKey: ["advProFaseses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-fases", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProFases(id: string) {
  return useQuery({
    queryKey: ["advProFases", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-fases/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProFases() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-fases", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProFaseses"] });
    },
  });
}

export function useUpdateAdvProFases() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-fases/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProFaseses"] });
      queryClient.invalidateQueries({ queryKey: ["advProFases", data.id] });
    },
  });
}

export function useDeleteAdvProFases() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-fases/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProFaseses"] });
    },
  });
}


