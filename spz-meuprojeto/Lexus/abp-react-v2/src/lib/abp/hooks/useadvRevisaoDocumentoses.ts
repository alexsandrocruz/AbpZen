import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvRevisaoDocumentosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvRevisaoDocumentoses(input: GetAdvRevisaoDocumentosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advRevisaoDocumentoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-revisao-documentos", {
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

export function useAllAdvRevisaoDocumentoses() {
  return useQuery({
    queryKey: ["advRevisaoDocumentoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-revisao-documentos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvRevisaoDocumentos(id: string) {
  return useQuery({
    queryKey: ["advRevisaoDocumentos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-revisao-documentos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvRevisaoDocumentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-revisao-documentos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advRevisaoDocumentoses"] });
    },
  });
}

export function useUpdateAdvRevisaoDocumentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-revisao-documentos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advRevisaoDocumentoses"] });
      queryClient.invalidateQueries({ queryKey: ["advRevisaoDocumentos", data.id] });
    },
  });
}

export function useDeleteAdvRevisaoDocumentos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-revisao-documentos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advRevisaoDocumentoses"] });
    },
  });
}


