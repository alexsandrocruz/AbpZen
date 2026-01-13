import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliPrioridadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliPrioridadeses(input: GetAdvCliPrioridadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliPrioridadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-prioridades", {
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

export function useAllAdvCliPrioridadeses() {
  return useQuery({
    queryKey: ["advCliPrioridadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-prioridades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliPrioridades(id: string) {
  return useQuery({
    queryKey: ["advCliPrioridades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-prioridades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliPrioridades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-prioridades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliPrioridadeses"] });
    },
  });
}

export function useUpdateAdvCliPrioridades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-prioridades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliPrioridadeses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliPrioridades", data.id] });
    },
  });
}

export function useDeleteAdvCliPrioridades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-prioridades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliPrioridadeses"] });
    },
  });
}


