import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProfissionaisesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProfissionaises(input: GetAdvProfissionaisesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProfissionaises", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais", {
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

export function useAllAdvProfissionaises() {
  return useQuery({
    queryKey: ["advProfissionaises", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProfissionais(id: string) {
  return useQuery({
    queryKey: ["advProfissionais", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-profissionais/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProfissionais() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-profissionais", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaises"] });
    },
  });
}

export function useUpdateAdvProfissionais() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-profissionais/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaises"] });
      queryClient.invalidateQueries({ queryKey: ["advProfissionais", data.id] });
    },
  });
}

export function useDeleteAdvProfissionais() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-profissionais/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaises"] });
    },
  });
}


