import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliComoChegousInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliComoChegous(input: GetAdvCliComoChegousInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliComoChegous", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-como-chegou", {
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

export function useAllAdvCliComoChegous() {
  return useQuery({
    queryKey: ["advCliComoChegous", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-como-chegou", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliComoChegou(id: string) {
  return useQuery({
    queryKey: ["advCliComoChegou", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-como-chegou/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliComoChegou() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-como-chegou", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliComoChegous"] });
    },
  });
}

export function useUpdateAdvCliComoChegou() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-como-chegou/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliComoChegous"] });
      queryClient.invalidateQueries({ queryKey: ["advCliComoChegou", data.id] });
    },
  });
}

export function useDeleteAdvCliComoChegou() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-como-chegou/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliComoChegous"] });
    },
  });
}


