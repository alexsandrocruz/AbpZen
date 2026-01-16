import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProcessosAlteracoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProcessosAlteracoeses(input: GetAdvProcessosAlteracoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProcessosAlteracoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-alteracoes", {
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

export function useAllAdvProcessosAlteracoeses() {
  return useQuery({
    queryKey: ["advProcessosAlteracoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-processos-alteracoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProcessosAlteracoes(id: string) {
  return useQuery({
    queryKey: ["advProcessosAlteracoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-processos-alteracoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProcessosAlteracoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-processos-alteracoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosAlteracoeses"] });
    },
  });
}

export function useUpdateAdvProcessosAlteracoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-processos-alteracoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosAlteracoeses"] });
      queryClient.invalidateQueries({ queryKey: ["advProcessosAlteracoes", data.id] });
    },
  });
}

export function useDeleteAdvProcessosAlteracoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-processos-alteracoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProcessosAlteracoeses"] });
    },
  });
}


