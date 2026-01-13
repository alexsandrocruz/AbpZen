import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinContasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinContases(input: GetFinContasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finContases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-contas", {
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

export function useAllFinContases() {
  return useQuery({
    queryKey: ["finContases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-contas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinContas(id: string) {
  return useQuery({
    queryKey: ["finContas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-contas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-contas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finContases"] });
    },
  });
}

export function useUpdateFinContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-contas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finContases"] });
      queryClient.invalidateQueries({ queryKey: ["finContas", data.id] });
    },
  });
}

export function useDeleteFinContas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-contas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finContases"] });
    },
  });
}


