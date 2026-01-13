import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvCliTiposArquivosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvCliTiposArquivoses(input: GetAdvCliTiposArquivosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advCliTiposArquivoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-arquivos", {
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

export function useAllAdvCliTiposArquivoses() {
  return useQuery({
    queryKey: ["advCliTiposArquivoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-cli-tipos-arquivos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvCliTiposArquivos(id: string) {
  return useQuery({
    queryKey: ["advCliTiposArquivos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-cli-tipos-arquivos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-cli-tipos-arquivos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}

export function useUpdateAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-cli-tipos-arquivos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivos", data.id] });
    },
  });
}

export function useDeleteAdvCliTiposArquivos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-cli-tipos-arquivos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advCliTiposArquivoses"] });
    },
  });
}


