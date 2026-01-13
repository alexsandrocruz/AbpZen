import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetFinGruposDREsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useFinGruposDREs(input: GetFinGruposDREsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["finGruposDREs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-grupos-dre", {
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

export function useAllFinGruposDREs() {
  return useQuery({
    queryKey: ["finGruposDREs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/fin-grupos-dre", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useFinGruposDRE(id: string) {
  return useQuery({
    queryKey: ["finGruposDRE", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/fin-grupos-dre/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateFinGruposDRE() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/fin-grupos-dre", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finGruposDREs"] });
    },
  });
}

export function useUpdateFinGruposDRE() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/fin-grupos-dre/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["finGruposDREs"] });
      queryClient.invalidateQueries({ queryKey: ["finGruposDRE", data.id] });
    },
  });
}

export function useDeleteFinGruposDRE() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/fin-grupos-dre/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["finGruposDREs"] });
    },
  });
}


