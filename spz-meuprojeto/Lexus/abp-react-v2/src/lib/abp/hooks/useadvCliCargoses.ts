import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliCargosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliCargoses(input: GetAdvCliCargosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliCargoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-cargos", {
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

export function useAllAdvCliCargoses() {
  return useQuery({
    queryKey: ["advCliCargoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-cargos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliCargos(id: string) {
  return useQuery({
    queryKey: ["advCliCargos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-cargos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-cargos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliCargoses"] });
    },
  });
}

export function useUpdateAdvCliCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-cargos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliCargoses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliCargos", data.id] });
    },
  });
}

export function useDeleteAdvCliCargos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-cargos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliCargoses"] });
    },
  });
}


