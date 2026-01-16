import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosDadosHerdeirosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProcessosDadosHerdeiroses(input: GetAdvProcessosDadosHerdeirosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessosDadosHerdeiroses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-dados-herdeiros", {
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

export function useAllAdvProcessosDadosHerdeiroses() {
  return useQuery({
    queryKey: ["advProcessosDadosHerdeiroses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-dados-herdeiros", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessosDadosHerdeiros(id: string) {
  return useQuery({
    queryKey: ["advProcessosDadosHerdeiros", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos-dados-herdeiros/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessosDadosHerdeiros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos-dados-herdeiros", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosDadosHerdeiroses"] });
    },
  });
}

export function useUpdateAdvProcessosDadosHerdeiros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos-dados-herdeiros/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosDadosHerdeiroses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessosDadosHerdeiros", data.id] });
    },
  });
}

export function useDeleteAdvProcessosDadosHerdeiros() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos-dados-herdeiros/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosDadosHerdeiroses"] });
    },
  });
}


