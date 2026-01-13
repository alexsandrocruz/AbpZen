import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvPreMetasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvPreMetases(input: GetAdvPreMetasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advPreMetases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-metas", {
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

export function useAllAdvPreMetases() {
  return useQuery({
    queryKey: ["advPreMetases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pre-metas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvPreMetas(id: string) {
  return useQuery({
    queryKey: ["advPreMetas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pre-metas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvPreMetas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pre-metas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreMetases"] });
    },
  });
}

export function useUpdateAdvPreMetas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pre-metas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advPreMetases"] });
      queryClient.invalidateQueries({ queryKey: ["advPreMetas", data.id] });
    },
  });
}

export function useDeleteAdvPreMetas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pre-metas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advPreMetases"] });
    },
  });
}


