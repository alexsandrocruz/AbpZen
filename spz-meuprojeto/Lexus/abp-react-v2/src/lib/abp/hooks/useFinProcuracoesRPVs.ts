import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinProcuracoesRPVsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinProcuracoesRPVs(input: GetFinProcuracoesRPVsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finProcuracoesRPVs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-procuracoes-rpv", {
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

export function useAllFinProcuracoesRPVs() {
  return useQuery({
    queryKey: ["finProcuracoesRPVs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-procuracoes-rpv", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinProcuracoesRPV(id: string) {
  return useQuery({
    queryKey: ["finProcuracoesRPV", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-procuracoes-rpv/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinProcuracoesRPV() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-procuracoes-rpv", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finProcuracoesRPVs"] });
    },
  });
}

export function useUpdateFinProcuracoesRPV() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-procuracoes-rpv/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finProcuracoesRPVs"] });
      queryClient.invalidateQueries({ queryKey: ["finProcuracoesRPV", data.id] });
    },
  });
}

export function useDeleteFinProcuracoesRPV() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-procuracoes-rpv/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finProcuracoesRPVs"] });
    },
  });
}


