import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProProbabilidadesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProProbabilidadeses(input: GetAdvProProbabilidadesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProProbabilidadeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-probabilidades", {
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

export function useAllAdvProProbabilidadeses() {
  return useQuery({
    queryKey: ["advProProbabilidadeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-probabilidades", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProProbabilidades(id: string) {
  return useQuery({
    queryKey: ["advProProbabilidades", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-probabilidades/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProProbabilidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-probabilidades", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProProbabilidadeses"] });
    },
  });
}

export function useUpdateAdvProProbabilidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-probabilidades/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProProbabilidadeses"] });
      queryClient.invalidateQueries({ queryKey: ["advProProbabilidades", data.id] });
    },
  });
}

export function useDeleteAdvProProbabilidades() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-probabilidades/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProProbabilidadeses"] });
    },
  });
}


