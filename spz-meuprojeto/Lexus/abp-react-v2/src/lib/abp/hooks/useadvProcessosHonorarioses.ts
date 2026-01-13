import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosHonorariosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProcessosHonorarioses(input: GetAdvProcessosHonorariosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessosHonorarioses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-honorarios", {
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

export function useAllAdvProcessosHonorarioses() {
  return useQuery({
    queryKey: ["advProcessosHonorarioses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-honorarios", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessosHonorarios(id: string) {
  return useQuery({
    queryKey: ["advProcessosHonorarios", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos-honorarios/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessosHonorarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos-honorarios", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosHonorarioses"] });
    },
  });
}

export function useUpdateAdvProcessosHonorarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos-honorarios/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosHonorarioses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessosHonorarios", data.id] });
    },
  });
}

export function useDeleteAdvProcessosHonorarios() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos-honorarios/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosHonorarioses"] });
    },
  });
}


