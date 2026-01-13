import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinLancamentos_BKPsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinLancamentos_BKPs(input: GetFinLancamentos_BKPsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finLancamentos_BKPs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-lancamentos-bkp", {
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

export function useAllFinLancamentos_BKPs() {
  return useQuery({
    queryKey: ["finLancamentos_BKPs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-lancamentos-bkp", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinLancamentos_BKP(id: string) {
  return useQuery({
    queryKey: ["finLancamentos_BKP", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-lancamentos-bkp/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinLancamentos_BKP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-lancamentos-bkp", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentos_BKPs"] });
    },
  });
}

export function useUpdateFinLancamentos_BKP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-lancamentos-bkp/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentos_BKPs"] });
      queryClient.invalidateQueries({ queryKey: ["finLancamentos_BKP", data.id] });
    },
  });
}

export function useDeleteFinLancamentos_BKP() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-lancamentos-bkp/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finLancamentos_BKPs"] });
    },
  });
}


