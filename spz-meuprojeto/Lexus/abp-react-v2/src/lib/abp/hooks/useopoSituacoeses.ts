import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetOpoSituacoesesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useOpoSituacoeses(input: GetOpoSituacoesesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["opoSituacoeses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-situacoes", {
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

export function useAllOpoSituacoeses() {
  return useQuery({
    queryKey: ["opoSituacoeses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/opo-situacoes", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useOpoSituacoes(id: string) {
  return useQuery({
    queryKey: ["opoSituacoes", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/opo-situacoes/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateOpoSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/opo-situacoes", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoSituacoeses"] });
    },
  });
}

export function useUpdateOpoSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/opo-situacoes/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["opoSituacoeses"] });
      queryClient.invalidateQueries({ queryKey: ["opoSituacoes", data.id] });
    },
  });
}

export function useDeleteOpoSituacoes() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/opo-situacoes/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["opoSituacoeses"] });
    },
  });
}


