import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  advProcessosClientesId?: string;
  advProcessosHonorariosId?: string;
  advProcessosMeritosId?: string;
  advCompromissosId?: string;
  advProcessosAlteracoesId?: string;
  advProcessosDadosHerdeirosId?: string;
  advTarefasId?: string;
  advVerbasId?: string;
  }

export function useAdvProcessoses(input: GetAdvProcessosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos", {
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

export function useAllAdvProcessoses() {
  return useQuery({
    queryKey: ["advProcessoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessos(id: string) {
  return useQuery({
    queryKey: ["advProcessos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessoses"] });
    },
  });
}

export function useUpdateAdvProcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessoses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessos", data.id] });
    },
  });
}

export function useDeleteAdvProcessos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessoses"] });
    },
  });
}


